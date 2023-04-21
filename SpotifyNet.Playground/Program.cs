using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Auth;
using SpotifyNet.Clients.Authorization;
using SpotifyNet.Clients.Interfaces;
using SpotifyNet.Clients.WebAPI;
using SpotifyNet.Common;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Repositories.Authorization;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Repositories.WebAPI;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.WebAPI;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        builder.ConfigureServices(services =>
        {
            services
            .AddSingleton<HttpClient>()
            .AddSingleton<IAuthorizationClient, AuthorizationClient>(p =>
            {
                var httpClient = p.GetRequiredService<HttpClient>();

                return new AuthorizationClient(AppClientId, AppRedirectUri, httpClient);
            })
            .AddSingleton<IAuthorizationRepository, AuthorizationRepository>()
            .AddSingleton<IAuthorizationService, AuthorizationService>()
            .AddSingleton<IWebAPIClient, WebAPIClient>()
            .AddSingleton<IWebAPIRepository, WebAPIRepository>()
            .AddSingleton<IWebAPIService, WebAPIService>()

            .AddSingleton(p =>
            {
                var configuration = p.GetRequiredService<IConfiguration>();

                var httpListener = new HttpListener();

                var appRedirectUri = AppRedirectUri;
                if (appRedirectUri.EndsWith('/'))
                {
                    httpListener.Prefixes.Add(appRedirectUri);
                }
                else
                {
                    httpListener.Prefixes.Add(appRedirectUri + '/');
                }

                return httpListener;
            })
            .AddSingleton<ITokenAcquirer, TokenAcquirer>();
        });

        var host = builder.Build();

        return Foo(host.Services);
    }

    private static async Task Foo(IServiceProvider serviceProvider)
    {
        var scopes = new[] { AuthorizationScope.UserLibraryRead, AuthorizationScope.PlaylistReadPrivate, AuthorizationScope.PlaylistReadCollaborative };

        var tokenAcquirer = serviceProvider.GetRequiredService<ITokenAcquirer>();
        await tokenAcquirer.EnsureTokenExists(scopes);

        var webAPIService = serviceProvider.GetRequiredService<IWebAPIService>();

        //var artistId = "2RIrl9cApI8HwM6aF4Jt5m";
        //var playlists = await webAPIService.GetCurrentUserPlaylists();

        //foreach (var playlist in playlists)
        //{
        //    if (playlist.Owner!.Id != "ziyad.ss" || playlist.Tracks!.Total == 0)
        //    {
        //        Console.WriteLine($"Skipped {playlist.Name} - {playlist.Uri}");
        //    }

        //    var tracks = await webAPIService.GetPlaylistTracks(playlist.Id!);
        //    var condition = tracks.Any(t => t.Track!.Artists!.Any(a => a.Id == artistId));

        //    if (condition)
        //    {
        //        Console.WriteLine($"{playlist.Name} - {playlist.Uri}");
        //    }
        //}

        var userId = "ziyad.ss";
        var playlists = await webAPIService.GetUserPlaylists(userId);

        foreach (var playlist in playlists)
        {
            if (playlist.Owner!.Id != userId || playlist.Tracks!.Total == 0)
            {
                Console.WriteLine($"Skipped {playlist.Name} - {playlist.Uri}");
            }

            var tracks = await webAPIService.GetPlaylistTracks(playlist.Id!);
            var condition = tracks.Any(t => t.AddedAt!.Value >= new DateTime(year: 2023, month: 4, day: 19));

            if (condition)
            {
                Console.WriteLine($"Found {playlist.Name} - {playlist.Uri}");
            }
        }
    }

    private static async Task<T> Read<T>(string fileName)
    {
        using var fs = File.OpenRead(fileName);
        var item = await JsonSerializer.DeserializeAsync<T>(fs);
        return item!;
    }

    private static Task Write<T>(string fileName, T item)
    {
        File.Delete(fileName);
        using var fs = File.OpenWrite(fileName);
        return JsonSerializer.SerializeAsync(fs, item);
    }
}
