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
    private static IServiceCollection AddHttpClient(
        this IServiceCollection services) => services.AddSingleton<HttpClient>();

    private static IServiceCollection AddAuthorization(
        this IServiceCollection services,
        string appClientId,
        string appRedirectUri) => services
        .AddSingleton<IAuthorizationClient, AuthorizationClient>(p =>
        {
            var httpClient = p.GetRequiredService<HttpClient>();

            return new AuthorizationClient(appClientId, appRedirectUri, httpClient);
        })
        .AddSingleton<IAuthorizationRepository, AuthorizationRepository>()
        .AddSingleton<IAuthorizationService, AuthorizationService>();

    private static IServiceCollection AddTokenAcquirer(
        this IServiceCollection services,
        string appRedirectUri) => services
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

    private static IServiceCollection AddWebAPI(
        this IServiceCollection services) => services
        .AddSingleton<IWebAPIClient, WebAPIClient>()
        .AddSingleton<IWebAPIRepository, WebAPIRepository>()
        .AddSingleton<IWebAPIService, WebAPIService>();


    internal static IServiceCollection AddSpotifyNetServices(
        this IServiceCollection services,
        string appClientId,
        string appRedirectUri) => services
        .AddHttpClient()
        .AddAuthorization(appClientId, appRedirectUri)
        .AddTokenAcquirer(appRedirectUri)
        .AddWebAPI();
}
