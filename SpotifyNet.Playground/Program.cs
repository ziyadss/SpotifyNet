using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Auth;
using SpotifyNet.Auth.Interfaces;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.WebAPI;
using SpotifyNet.WebAPI.Interfaces;
using System;
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

        var albumIds = new[] { "0e9GjrztzBw8oMC6n2CDeI", "6zxHzgT0fKSMEgIi7BpoyQ" };

        var result = await webAPIRepository.AreAlbumsSaved(albumIds, accessToken);
        var resultString = string.Join(',', result);

        Console.WriteLine(resultString);
    }
}
