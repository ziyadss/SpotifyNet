using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace SpotifyNet.Common;

public static class RegistrationExtensions
{
    public static IServiceCollection AddTokenAcquirer(this IServiceCollection services) => services.AddSingleton<ITokenAcquirer, TokenAcquirer>();

    public static IServiceCollection AddRedirectUriListener(
        this IServiceCollection services,
        string appRedirectUri) => services.AddSingleton(p =>
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
        });
}
