using Microsoft.Extensions.DependencyInjection;
using SpotifyNet.Auth;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.WebAPI;

namespace SpotifyNet.Services;

public static class RegistrationExtensions
{
    public static IServiceCollection AddAuthorizationService(this IServiceCollection services) => services.AddSingleton<IAuthorizationService, AuthorizationService>();

    public static IServiceCollection AddWebAPIService(this IServiceCollection services) => services.AddSingleton<IWebAPIService, WebAPIService>();
}
