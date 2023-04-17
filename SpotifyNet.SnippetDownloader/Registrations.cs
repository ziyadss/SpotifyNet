using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotifyNet.Auth;
using SpotifyNet.Clients.Authorization;
using SpotifyNet.Clients.Interfaces;
using SpotifyNet.Clients.WebAPI;
using SpotifyNet.Common;
using SpotifyNet.Repositories.Authorization;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Repositories.WebAPI;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.WebAPI;
using System.Net;

namespace SpotifyNet.SnippetDownloader;

internal static class Registrations
{
    internal static IServiceCollection AddRegistrations(this IServiceCollection services) => services
            .AddSingleton<IAuthorizationClient, AuthorizationClient>(p =>
            {
                var configuration = p.GetRequiredService<IConfiguration>();

                var appClientId = configuration["appClientId"]!;
                var appRedirectUri = configuration["appRedirectUri"]!;

                return new AuthorizationClient(appClientId, appRedirectUri);
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

                var appRedirectUri = configuration["appRedirectUri"]!;
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
}
