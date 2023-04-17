using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Common;
using SpotifyNet.Datastructures.Spotify.Authorization;
using System;
using System.Collections.Generic;
using System.CommandLine;
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

        var appClientIdOption = new Option<string>(
            name: "--app-client-id",
            description: "The Spotify Developer App's client id.");

        var appRedirectUriOption = new Option<string>(
            name: "--app-redirect-uri",
            description: "The Spotify Developer App's redirect id.");

        var outputDirectoryOption = new Option<DirectoryInfo>(
            name: "--output-directory",
            description: "The directory to place the downloader's outputs.");

        var newTokenOption = new Option<bool>(
            name: "--force-generate-token",
            description: "Whether to force the generation of a new access token.",
            getDefaultValue: () => false);

        var trackIdOption = new Option<string>(
            name: "--track-id",
            description: "The id of the track to download.");

        var playlistIdOption = new Option<string>(
            name: "--playlist-id",
            description: "The id of the playlist to download.");

        var trackCommand = new Command("track", "Download a track.")
        {
            appClientIdOption,
            appRedirectUriOption,
            outputDirectoryOption,
            newTokenOption,
            trackIdOption,
        };
        trackCommand.SetHandler(async (trackId, newToken) => await DownloadTrack(newToken, trackId, host.Services), trackIdOption, newTokenOption);

        var playlistCommand = new Command("playlist", "Download a playlist.")
        {
            appClientIdOption,
            appRedirectUriOption,
            outputDirectoryOption,
            newTokenOption,
            playlistIdOption,
        };
        playlistCommand.SetHandler(async (playlistId, newToken) => await DownloadPlaylist(newToken, playlistId, host.Services), playlistIdOption, newTokenOption);

        var rootCommand = new RootCommand("Snippet Downloader using Spotify APIs.")
        {
            appClientIdOption,
            appRedirectUriOption,
            outputDirectoryOption,
            newTokenOption,
            trackCommand,
            playlistCommand
        };

        await rootCommand.InvokeAsync(args);
    }

    private static async Task DownloadTrack(
        bool newAccessToken,
        string trackId,
        IServiceProvider serviceProvider)
    {
        var snippetDownloader = await GetSnippetDownloader(newAccessToken, serviceProvider);

        var (fileName, status) = await snippetDownloader.DownloadTrack(trackId);

        var output = new Dictionary<string, SnippetDownloadStatus> { [fileName] = status };

        await WriteOutput(serviceProvider, output);
    }

    private static async Task DownloadPlaylist(
        bool newAccessToken,
        string playlistId,
        IServiceProvider serviceProvider)
    {
        var snippetDownloader = await GetSnippetDownloader(newAccessToken, serviceProvider);

        var result = await snippetDownloader.DownloadPlaylist(playlistId);

        var output = result.ToDictionary(pair => pair.FileName, pair => pair.Status);

        await WriteOutput(serviceProvider, output);
    }

    private static async Task<ISnippetDownloader> GetSnippetDownloader(
        bool newAccessToken,
        IServiceProvider serviceProvider)
    {
        var scopes = new[] { AuthorizationScope.UserLibraryRead, AuthorizationScope.PlaylistReadPrivate };

        var tokenAcquirer = serviceProvider.GetRequiredService<ITokenAcquirer>();
        await tokenAcquirer.EnsureTokenExists(scopes, newAccessToken);

        return serviceProvider.GetRequiredService<ISnippetDownloader>();
    }

    private static async Task WriteOutput(
        IServiceProvider serviceProvider,
        IReadOnlyDictionary<string, SnippetDownloadStatus> output)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var outputDirectory = configuration["outputDirectory"]!;

        Directory.CreateDirectory(outputDirectory);
        var outputFilePath = Path.Combine(outputDirectory, "output.json");
        await Write(outputFilePath, output);
    }

    private static async Task Write<T>(
        string fileName,
        T item)
    {
        File.Delete(fileName);
        using var fs = File.OpenWrite(fileName);
        await JsonSerializer.SerializeAsync(fs, item);
    }
}
