using Microsoft.Extensions.DependencyInjection;
using SpotifyNet.Services.Interfaces;

namespace SpotifyNet.Playground;

internal static class RegistrationExtensions
{
    public static IServiceCollection AddTokenAcquirer(
        this IServiceCollection services,
        string appRedirectUri) => services.AddSingleton<ITokenAcquirer, TokenAcquirer>(p =>
        {
            var authorizationService = p.GetRequiredService<IAuthorizationService>();

            return new TokenAcquirer(appRedirectUri, authorizationService);
        });
}
