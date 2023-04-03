using Microsoft.Extensions.DependencyInjection;
using SpotifyNet.WebAPI.Interfaces;

namespace SpotifyNet.WebAPI;

public static class RegistrationExtensions
{
    public static IServiceCollection AddWebAPIService(this IServiceCollection services)
    {
        services.AddSingleton<IWebAPIClient, WebAPIClient>();

        services.AddSingleton<IWebAPIRepository, WebAPIRepository>();

        return services;
    }
}
