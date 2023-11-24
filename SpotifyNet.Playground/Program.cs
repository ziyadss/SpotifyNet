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
            AuthorizationScope.UserTopRead,
            AuthorizationScope.UserLibraryRead,
            AuthorizationScope.PlaylistReadPrivate,
            AuthorizationScope.UserModifyPlaybackState,
            AuthorizationScope.UserReadCurrentlyPlaying,
            AuthorizationScope.PlaylistModifyPrivate,
            AuthorizationScope.PlaylistModifyPublic,
        };

        await tokenAcquirerService.EnsureTokenExists(scopes);

        //await webAPIService.Player.SetPlaybackVolume(55);

        var userId = "ziyad.ss";
        var playlists = await webAPIService.Playlists.GetCurrentUserPlaylists();
        static bool condition(PlaylistTrack t) => t.Track!.Name!.Contains("Waiting Room");

        foreach (var playlist in playlists)
        {
            if (playlist.Owner!.Id != userId)
            {
                Console.WriteLine($"Skipped {playlist.Name} ({playlist.Uri})");
                continue;
            }

            var tracks = await webAPIService.Playlists.GetPlaylistTracks(playlist.Id!);
            var result = tracks
                .Where(condition)
                .Select(FullTrackName)
                .ToList();

            if (result.Count != 0)
            {
                Console.WriteLine($"Found {playlist.Name} ({playlist.Uri}), has: ");
                foreach (var track in result)
                {
                    Console.WriteLine($"    {track}");
                }
            }
        }
    }

    private static string FullTrackName(PlaylistTrack playlistTrack) => FullTrackName(playlistTrack.Track!);

    private static string FullTrackName(Track track)
    {
        var trackName = track.Name;

        var artists = track.Artists?.Select(a => a.Name) ?? Array.Empty<string>();
        var artistsNames = string.Join(", ", artists);

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
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        using var fs = File.OpenWrite(path);
        await JsonSerializer.SerializeAsync(fs, item);
    }
}
