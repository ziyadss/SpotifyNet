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
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyNet.Playground;

sealed internal class Program
{
    private const string AppClientId = "";
    private const string AppRedirectUri = "http://localhost:3000";

    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services =>
        {
            services
            .AddSingleton<IAuthorizationClient, AuthorizationClient>(p => new AuthorizationClient(AppClientId, AppRedirectUri))
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

        await Test(host.Services);
    }

    private static async Task Test(IServiceProvider serviceProvider)
    {
        var newToken = true;
        var scopes = new[] { AuthorizationScope.UserLibraryRead, AuthorizationScope.PlaylistReadPrivate };

        if (newToken)
        {
            var tokenAcquirer = serviceProvider.GetRequiredService<ITokenAcquirer>();
            await tokenAcquirer.GenerateToken(scopes);
        }

        var webAPIService = serviceProvider.GetRequiredService<IWebAPIService>();

        var savedTracks = await webAPIService.GetCurrentUserSavedTracks();

        Console.WriteLine(savedTracks.Count());
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
