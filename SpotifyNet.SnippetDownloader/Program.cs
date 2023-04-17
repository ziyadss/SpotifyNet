using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Common;
using SpotifyNet.Datastructures.Spotify.Authorization;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyNet.SnippetDownloader;

sealed internal class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services => services.AddRegistrations());

        var host = builder.Build();

        await Run(host.Services);
    }

    private static async Task Run(IServiceProvider serviceProvider)
    {
        var newToken = true;
        var scopes = new[] { AuthorizationScope.PlaylistReadPrivate };

        if (newToken)
        {
            var tokenAcquirer = serviceProvider.GetRequiredService<ITokenAcquirer>();
            await tokenAcquirer.GenerateToken(scopes);
        }

        var snippetDownloader = serviceProvider.GetRequiredService<ISnippetDownloader>();

        var result = await snippetDownloader.DownloadPlaylist("5aMKlx7LBLKPLUVukF6X0M");

        var output = result.ToDictionary(pair => pair.TrackName, pair => pair.Status);


        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var outputDirectory = configuration["outputDirectory"]!;

        var outputFilePath = Path.Combine(outputDirectory, "output.json");
        await Write(outputFilePath, output);
    }

    private static async Task Write<T>(string fileName, T item)
    {
        File.Delete(fileName);
        using var fs = File.OpenWrite(fileName);
        await JsonSerializer.SerializeAsync(fs, item);
    }
}
