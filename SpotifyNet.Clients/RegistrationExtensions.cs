using Microsoft.Extensions.DependencyInjection;
using SpotifyNet.Clients.Authorization;
using SpotifyNet.Clients.Interfaces;
using SpotifyNet.Clients.WebAPI;

namespace SpotifyNet.Clients;

public static class RegistrationExtensions
{
    public static IServiceCollection AddAuthorizationClient(
        this IServiceCollection services,
        string appClientId,
        string appRedirectUri) => services.AddSingleton<IAuthorizationClient, AuthorizationClient>(p => new AuthorizationClient(appClientId, appRedirectUri));

    public static IServiceCollection AddWebAPIClient(this IServiceCollection services) => services.AddSingleton<IWebAPIClient, WebAPIClient>();
}
