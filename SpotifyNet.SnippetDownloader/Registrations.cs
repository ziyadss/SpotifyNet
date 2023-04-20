using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotifyNet.Auth;
using SpotifyNet.Clients.Authorization;
using SpotifyNet.Clients.Interfaces;
using SpotifyNet.Clients.WebAPI;
using SpotifyNet.Common;
using SpotifyNet.Repositories.Authorization;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Repositories.WebAPI;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.WebAPI;
using System.IO;
using System.Net;
using System.Net.Http;

namespace SpotifyNet.SnippetDownloader;

internal static class Registrations
{
    internal static IServiceCollection AddRegistrations(this IServiceCollection services) => services
        .AddSingleton<HttpClient>()
        .AddSingleton<IAuthorizationClient, AuthorizationClient>(p =>
        {
            var configuration = p.GetRequiredService<IConfiguration>();

            var appClientId = configuration["app-client-id"]!;
            var appRedirectUri = configuration["app-redirect-uri"]!;

            var httpClient = p.GetRequiredService<HttpClient>();

            return new AuthorizationClient(appClientId, appRedirectUri, httpClient);
        })
        .AddSingleton<IAuthorizationRepository, AuthorizationRepository>()
        .AddSingleton<IAuthorizationService, AuthorizationService>()
        .AddSingleton<IWebAPIClient, WebAPIClient>()
        .AddSingleton<IWebAPIRepository, WebAPIRepository>()
        .AddSingleton<IWebAPIService, WebAPIService>()

        .AddSingleton(p =>
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
        .AddSingleton<ITokenAcquirer, TokenAcquirer>()

        .AddSingleton<ISnippetDownloader, SnippetDownloader>(p =>
        {
            var configuration = p.GetRequiredService<IConfiguration>();

            var outputDirectory = configuration["output-directory"]!;
            var snippetsDirectory = Path.Combine(outputDirectory, "files");

            var webAPIService = p.GetRequiredService<IWebAPIService>();
            var httpClient = p.GetRequiredService<HttpClient>();

            return new SnippetDownloader(snippetsDirectory, webAPIService, httpClient);
        });
}
