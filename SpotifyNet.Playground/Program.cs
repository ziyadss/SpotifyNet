using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyNet.Playground;

sealed internal class Program
{
    private const string AppClientId = "";
    private const string AppRedirectUri = "http://localhost:3000";

    public static Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services => services.AddRegistrations(AppClientId, AppRedirectUri));

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
            var fulfilled = tracks.Any(condition);

            if (fulfilled)
            {
                Console.WriteLine($"{playlist.Name} - {playlist.Uri}");
            }
        }
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
