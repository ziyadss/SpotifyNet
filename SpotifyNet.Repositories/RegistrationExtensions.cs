using Microsoft.Extensions.DependencyInjection;
using SpotifyNet.Repositories.Authorization;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Repositories.WebAPI;

namespace SpotifyNet.Repositories;

public static class RegistrationExtensions
{
    public static IServiceCollection AddAuthorizationRepository(this IServiceCollection services) => services.AddSingleton<IAuthorizationRepository, AuthorizationRepository>();

    public static IServiceCollection AddWebAPIRepository(this IServiceCollection services) => services.AddSingleton<IWebAPIRepository, WebAPIRepository>();
}
