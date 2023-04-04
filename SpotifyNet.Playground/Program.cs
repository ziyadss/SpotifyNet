using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Auth;
using SpotifyNet.Auth.Interfaces;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.WebAPI;
using SpotifyNet.WebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyNet.Playground;

internal class Program
{
    private const string AppClientId = "";
    private const string AppRedirectUri = "http://localhost:3000";

    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services =>
        {
            services
            .AddAuthorizationService(AppClientId, AppRedirectUri)
            .AddWebAPIService();

            AddTokenAcquirer(services, AppRedirectUri);
        });

        var host = builder.Build();

        await Test(host.Services);
    }

    private static IServiceCollection AddTokenAcquirer(IServiceCollection services, string appRedirectUri)
    {
        services.AddSingleton<ITokenAcquirer, TokenAcquirer>(p =>
        {
            var authorizationService = p.GetRequiredService<IAuthorizationService>();

            return new TokenAcquirer(appRedirectUri, authorizationService);
        });

        return services;
    }

    private static async Task Test(IServiceProvider serviceProvider)
    {
        var newToken = false;
        var scopes = new[] { AuthorizationScope.UserLibraryRead, AuthorizationScope.PlaylistReadPrivate };

        var tokenAcquirer = serviceProvider.GetRequiredService<ITokenAcquirer>();

        var accessToken = newToken ? await tokenAcquirer.GetToken(scopes) : await tokenAcquirer.GetExistingToken();

        var webAPIRepository = serviceProvider.GetRequiredService<IWebAPIRepository>();

        var playlistUrl = "https://open.spotify.com/playlist/6XUE4OxR8LtmVpxOnC11aX?si=31b9e163ede04b23&pt=ecaff3da232c0808bc736b4402773bfc";
        var playlistId = new Uri(playlistUrl).AbsolutePath.Split('/').Last();

        var tracks = await webAPIRepository.GetPlaylistItems(playlistId, accessToken);

        var chunks = tracks.Select(t => t.Track!.Id!).Chunk(100);

        var features = new List<AudioFeatures>();
        foreach (var chunk in chunks)
        {
            var chunkFeatures = await webAPIRepository.GetTracksAudioFeatures(chunk, accessToken);
            features.AddRange(chunkFeatures.Where(cf => cf is not null));
        }

        var processedFeatures = features.Join(
            tracks,
            f => f.Id,
            t => t.Track!.Id,
            (f, t) => new
            {
                name = $"{t.Track!.Name} - {string.Join(',', t.Track.Artists!.Select(a => a.Name))}",
                acousticness = f.Acousticness!.Value,
                danceability = f.Danceability!.Value,
                energy = f.Energy!.Value,
                instrumentalness = f.Instrumentalness!.Value,
                key = f.Key!.Value,
                liveness = f.Liveness!.Value,
                loudness = f.Loudness!.Value,
                mode = f.Mode!.Value,
                speechiness = f.Speechiness!.Value,
                tempo = f.Tempo!.Value,
                time_signature = f.TimeSignature!.Value,
                valence = f.Valence!.Value,
            }).ToArray();

        await Write("tracks.json", processedFeatures);
    }

    private static async Task<T> Read<T>(string fileName)
    {
        using var fs = File.OpenRead(fileName);
        var item = await JsonSerializer.DeserializeAsync<T>(fs);
        return item!;
    }

    private static async Task Write<T>(string fileName, T item)
    {
        File.Delete(fileName);
        using var fs = File.OpenWrite(fileName);
        await JsonSerializer.SerializeAsync(fs, item);
    }
}
