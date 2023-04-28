using Microsoft.Extensions.DependencyInjection;
using SpotifyNet.Clients.Authorization;
using SpotifyNet.Clients.Interfaces;
using SpotifyNet.Clients.WebAPI;
using SpotifyNet.Repositories.Authorization;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Repositories.WebAPI;
using SpotifyNet.Services.Authorization;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.Services.WebAPI;
using System.Net;
using System.Net.Http;

namespace SpotifyNet.Playground;

internal static class Registrations
{
    internal static IServiceCollection AddRegistrations(
        this IServiceCollection services,
        string appClientId,
        string appRedirectUri) => services
        .AddSingleton<HttpClient>()
        .AddSingleton<IAuthorizationClient, AuthorizationClient>(p =>
        {
            var httpClient = p.GetRequiredService<HttpClient>();

            return new AuthorizationClient(appClientId, appRedirectUri, httpClient);
        })
        .AddSingleton<IAuthorizationRepository, AuthorizationRepository>()
        .AddSingleton<IAuthorizationService, AuthorizationService>()
        .AddSingleton<IWebAPIClient, WebAPIClient>()
        .AddSingleton<IWebAPIRepository, WebAPIRepository>()
        .AddSingleton<IWebAPIService, WebAPIService>()

        .AddSingleton(p =>
        {
            var httpListener = new HttpListener();

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
        .AddSingleton<ITokenAcquirerService, TokenAcquirerService>();
}
