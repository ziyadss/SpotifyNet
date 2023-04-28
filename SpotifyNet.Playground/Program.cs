using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyNet.Playground;

internal sealed class Program
{
    private const string AppClientId = "";
    private const string AppRedirectUri = "http://localhost:3000";

    public static Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services => services.AddSpotifyNetServices(AppClientId, AppRedirectUri));

        var host = builder.Build();

        var tokenAcquirerService = host.Services.GetRequiredService<ITokenAcquirerService>();
        var webAPIService = host.Services.GetRequiredService<IWebAPIService>();

        return Foo(tokenAcquirerService, webAPIService);
    }

    private static async Task Foo(ITokenAcquirerService tokenAcquirerService, IWebAPIService webAPIService)
    {
        var scopes = new[]
        {
            AuthorizationScope.UserLibraryRead,
            AuthorizationScope.PlaylistReadPrivate,
            AuthorizationScope.PlaylistReadCollaborative,
            AuthorizationScope.UserReadCurrentlyPlaying,
            AuthorizationScope.UserTopRead,
        };

        await tokenAcquirerService.EnsureTokenExists(scopes);

        var userId = "ziyad.ss";
        var playlists = await webAPIService.Playlists.GetCurrentUserPlaylists();
        var condition = (PlaylistTrack t) => t.Track!.Artists!.Any(a => a.Id == "2RIrl9cApI8HwM6aF4Jt5m");

        foreach (var playlist in playlists)
        {
            if (playlist.Owner!.Id != userId)
            {
                Console.WriteLine($"Skipped {playlist.Name} - {playlist.Uri}");
            }

            var tracks = await webAPIService.Playlists.GetPlaylistTracks(playlist.Id!);
            var result = tracks
                .Where(condition)
                .Select(pt => FullTrackName(pt.Track!));

            if (result.Any())
            {
                Console.WriteLine($"Found {playlist.Name} ({playlist.Uri}), has: ");
                foreach (var track in result)
                {
                    Console.WriteLine($"    {track}");
                }
            }
        }
    }

    private static string FullTrackName(Track track)
    {
        var trackName = track.Name;
        var artistsNames = string.Join(", ", track.Artists?.Select(a => a.Name) ?? Array.Empty<string>());
        return $"{trackName} - {artistsNames}";
    }

    private static async Task<T> Read<T>(string path)
    {
        using var fs = File.OpenRead(path);
        var item = await JsonSerializer.DeserializeAsync<T>(fs);
        return item!;
    }

    private static async Task Write<T>(string path, T item)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            var directory = Path.GetDirectoryName(path)!;
            Directory.CreateDirectory(directory);
        }

        using var fs = File.OpenWrite(path);
        await JsonSerializer.SerializeAsync(fs, item);
    }
}
