using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Auth;
using SpotifyNet.Auth.Interfaces;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.WebAPI;
using SpotifyNet.WebAPI.Interfaces;
using System;
using System.Linq;
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
        var scopes = new[] { AuthorizationScope.PlaylistReadPrivate, AuthorizationScope.UserFollowRead };

        var tokenAcquirer = serviceProvider.GetRequiredService<ITokenAcquirer>();

        var token = newToken ? await tokenAcquirer.GetToken(scopes) : await tokenAcquirer.GetExistingToken();

        var webAPIRepository = serviceProvider.GetRequiredService<IWebAPIRepository>();

        var playlists = await webAPIRepository.GetCurrentUserPlaylists(token, ownerId: "ziyad.ss");

        Console.WriteLine(playlists[0].Name);
        Console.WriteLine(playlists[0].Tracks!.Total);

        var tracks = await webAPIRepository.GetPlaylistItems(token, playlists[0].Id!);
        Console.WriteLine(tracks.Count);
        Console.WriteLine(string.Join(',', tracks.Select(t => t.Track!.Name)));
    }
}
