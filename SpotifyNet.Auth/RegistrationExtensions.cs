using Microsoft.Extensions.DependencyInjection;
using SpotifyNet.Auth.Interfaces;

namespace SpotifyNet.Auth;

public static class RegistrationExtensions
{
    public static IServiceCollection AddAuthorizationService(this IServiceCollection services, string appClientId, string appRedirectUri)
    {
        services.AddSingleton<IAuthorizationClient, AuthorizationClient>(p => new AuthorizationClient(appClientId, appRedirectUri));

        services.AddSingleton<IAuthorizationRepository, AuthorizationRepository>();

        services.AddSingleton<IAuthorizationService, AuthorizationService>();

        return services;
    }
}
