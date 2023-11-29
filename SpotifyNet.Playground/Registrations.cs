using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using SpotifyNet.Clients;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Repositories;
using SpotifyNet.Repositories.Abstractions;
using SpotifyNet.Repositories.WebAPI;
using SpotifyNet.Services;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;
using SpotifyNet.Services.WebAPI;

namespace SpotifyNet.Playground;

internal static class Registrations
{
    private static IServiceCollection AddAuthorization(
        this IServiceCollection services,
        string appClientId,
        string appRedirectUri) => services.AddSingleton<IAuthorizationClient, AuthorizationClient>(p =>
                                           {
                                               var httpClient = p.GetRequiredService<HttpClient>();

                                               return new(appClientId, appRedirectUri, httpClient);
                                           })
                                          .AddSingleton<IAuthorizationRepository, AuthorizationRepository>()
                                          .AddSingleton<IAuthorizationService, AuthorizationService>();

    private static IServiceCollection AddTokenAcquirer(this IServiceCollection services, string appRedirectUri) =>
        services.AddSingleton(_ =>
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

    private static IServiceCollection AddWebAPI(this IServiceCollection services) => services
       .AddSingleton<IWebAPIClient, WebAPIClient>()
       .AddSingleton<IWebAPIRepository, WebAPIRepository>()
       .AddSingleton<IWebAPIService, WebAPIService>();

    internal static IServiceCollection AddSpotifyNetServices(
        this IServiceCollection services,
        string appClientId,
        string appRedirectUri) => services.AddHttpClient()
                                          .AddAuthorization(appClientId, appRedirectUri)
                                          .AddTokenAcquirer(appRedirectUri)
                                          .AddWebAPI();
}
