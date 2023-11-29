using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Playground;

internal sealed class Program
{
    private const string AppClientId = "";
    private const string AppRedirectUri = "http://localhost:3000";

    private static readonly JsonSerializerOptions indentedJsonOptions = new(JsonSerializerOptions.Default)
    {
        WriteIndented = true,
    };

    private static readonly JsonSerializerOptions compactJsonOptions = new(JsonSerializerOptions.Default)
    {
        WriteIndented = false,
    };

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

        await webAPIService.Player.SetPlaybackVolume(55);

        IReadOnlyList<SavedTrack> likedSongs;
        try
        {
            likedSongs = await Read<IReadOnlyList<SavedTrack>>("likedSongs.json");
        }
        catch
        {
            likedSongs = await webAPIService.Tracks.GetCurrentUserSavedTracks();
            await Write("likedSongs.json", likedSongs);
        }

        var likedSongsDto = likedSongs.Select(t => new
        {
            Song = t.Track!.Name!,
            Artists = string.Join(", ", t.Track?.Artists?.Select(a => a.Name) ?? Array.Empty<string>()),
            Album = t.Track?.Album?.Name ?? string.Empty,
        });

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var likedSongsPath = Path.Combine(desktopPath, "likedSongs.json");
        await Write(likedSongsPath, likedSongsDto);
    }

    private static string FullTrackName(PlaylistTrack playlistTrack) => FullTrackName(playlistTrack.Track!);

    private static string FullTrackName(SavedTrack savedTrack) => FullTrackName(savedTrack.Track!);

    private static string FullTrackName(Track track)
    {
        var trackName = track.Name;

        var artists = track.Artists?.Select(a => a.Name) ?? Array.Empty<string>();
        var artistsNames = string.Join(", ", artists);

        return $"{trackName} - {artistsNames}";
    }

    private static async Task<T> Read<T>(string path)
    {
        await using var fs = File.OpenRead(path);
        var item = await JsonSerializer.DeserializeAsync<T>(fs);
        return item!;
    }

    private static async Task Write<T>(string path, T item, bool indented = false)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        await using var fs = File.OpenWrite(path);

        var options = indented ? indentedJsonOptions : compactJsonOptions;
        await JsonSerializer.SerializeAsync(fs, item, options);
    }
}
