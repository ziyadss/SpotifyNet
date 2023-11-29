using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
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

namespace SpotifyNet.SnippetDownloader;

internal static class Registrations
{
    private static IServiceCollection AddAuthorization(this IServiceCollection services) => services
       .AddSingleton<IAuthorizationClient, AuthorizationClient>(p =>
        {
            var configuration = p.GetRequiredService<IConfiguration>();

            var appClientId = configuration["app-client-id"]!;
            var appRedirectUri = configuration["app-redirect-uri"]!;

            var httpClient = p.GetRequiredService<HttpClient>();

            return new(appClientId, appRedirectUri, httpClient);
        })
       .AddSingleton<IAuthorizationRepository, AuthorizationRepository>()
       .AddSingleton<IAuthorizationService, AuthorizationService>();

    private static IServiceCollection AddTokenAcquirer(this IServiceCollection services) => services.AddSingleton(p =>
        {
            var configuration = p.GetRequiredService<IConfiguration>();

            var httpListener = new HttpListener();

            var appRedirectUri = configuration["app-redirect-uri"]!;
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


    internal static IServiceCollection AddSpotifyNetServices(this IServiceCollection services) =>
        services.AddHttpClient().AddAuthorization().AddTokenAcquirer().AddWebAPI();

    internal static IServiceCollection AddSnippetDownloader(this IServiceCollection services) =>
        services.AddSingleton<ISnippetDownloader, SnippetDownloader>(p =>
        {
            var configuration = p.GetRequiredService<IConfiguration>();

            var outputDirectory = configuration["output-directory"]!;
            var snippetsDirectory = Path.Combine(outputDirectory, "files");

            var webAPIService = p.GetRequiredService<IWebAPIService>();
            var httpClient = p.GetRequiredService<HttpClient>();

            return new(snippetsDirectory, webAPIService, httpClient);
        });
}
