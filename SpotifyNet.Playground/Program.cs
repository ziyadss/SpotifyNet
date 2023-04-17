using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Clients;
using SpotifyNet.Common;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Repositories;
using SpotifyNet.Services;
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

    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services =>
        {
            services
            .AddAuthorizationClient(AppClientId, AppRedirectUri)
            .AddAuthorizationRepository()
            .AddAuthorizationService()
            .AddWebAPIClient()
            .AddWebAPIRepository()
            .AddWebAPIService()

            .AddRedirectUriListener(AppRedirectUri)
            .AddTokenAcquirer();
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
