using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Auth;
using SpotifyNet.Auth.Interfaces;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.WebAPI;
using SpotifyNet.WebAPI.Interfaces;
using System;
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
        var scopes = new[] { AuthorizationScope.UserLibraryRead };

        var tokenAcquirer = serviceProvider.GetRequiredService<ITokenAcquirer>();

        var accessToken = newToken ? await tokenAcquirer.GetToken(scopes) : await tokenAcquirer.GetExistingToken();

        var webAPIRepository = serviceProvider.GetRequiredService<IWebAPIRepository>();

        var savedTracks = await webAPIRepository.GetCurrentUserSavedTracks(accessToken);

        var chunkedIds = savedTracks.Select(st => st.Track!.Id!).Chunk(100).ToArray();
        await Write("chunkedIds.json", chunkedIds);

        //var chunkedIds = await Read<string[][]>("chunkedIds.json");

        //var features = new List<AudioFeatures>(chunkedIds.Sum(chunk => chunk.Length));
        //foreach (var chunk in chunkedIds)
        //{
        //    var chunkFeatures = await webAPIRepository.GetTracksAudioFeatures(chunk, accessToken);
        //    features.AddRange(chunkFeatures.Where(cf => cf is not null));
        //}

        //await Write("features.json", features.ToArray());

        //var features = await Read<AudioFeatures[]>("features.json");
        //var processedFeaturesList = new List<CustomAudioFeatures>(features.Length);
        //foreach (var feature in features)
        //{
        //    var custom = await MapAudioFeatures(feature, webAPIRepository, accessToken);
        //    if (custom is not null)
        //    {
        //        processedFeaturesList.Add(custom);
        //    }
        //}
        //var processedFeatures = processedFeaturesList.ToArray();
        //Console.WriteLine($"{processedFeatures.Length}/{features.Length}");
        //await Write("full-features-processed.json", processedFeatures);

        //var processedFeatures = await Read<CustomAudioFeatures[]>("full-features-processed.json");
        //Console.WriteLine(processedFeatures.Length);
    }

    private static async Task<T> Read<T>(string fileName)
    {
        using var fs = File.OpenRead(fileName);

        var item = await JsonSerializer.DeserializeAsync<T>(fs);

        return item!;
    }

    private static async Task Write<T>(string fileName, T item)
    {
        using var fs = File.OpenWrite(fileName);

        await JsonSerializer.SerializeAsync(fs, item);
    }
}
