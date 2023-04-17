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

internal sealed class Program
{
    private static readonly string[] _requiredScopes = new[] { AuthorizationScope.UserLibraryRead, AuthorizationScope.PlaylistReadPrivate };

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerOptions.Default)
    {
        WriteIndented = true,
    };

    public static async Task Main(
        string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services => services.AddRegistrations());

        var host = builder.Build();

        var rootCommand = GetRootCommand(host.Services);

        await rootCommand.InvokeAsync(args);
    }

    private static RootCommand GetRootCommand(
        IServiceProvider serviceProvider)
    {
        // Global options.
        var appClientIdOption = new Option<string>("--app-client-id", "The Spotify Developer App's client id.")
        {
            IsRequired = true,
        };

        var appRedirectUriOption = new Option<string>("--app-redirect-uri", "The Spotify Developer App's redirect id.")
        {
            IsRequired = true,
        };

        var outputDirectoryOption = new Option<DirectoryInfo>("--output-directory", "The directory to place the downloader's outputs.")
        {
            IsRequired = true,
        };

        var newTokenOption = new Option<bool>("--force-generate-token", () => false, "Whether to force the generation of a new access token.")
        {
            IsRequired = false,
        };

        var globalOptions = new Option[] { appClientIdOption, appRedirectUriOption, outputDirectoryOption, newTokenOption };

        // Track command options.
        var trackIdsOption = new Option<string[]>("--track-ids", "The ids of the tracks to download.")
        {
            IsRequired = true,
            AllowMultipleArgumentsPerToken = true,
        };

        var trackCommand = new Command("track", "Download a track.")
        {
            trackIdsOption,
        };

        trackCommand.SetHandler(
            async (trackIds, outputDirectory, newToken) => await DownloadTracks(newToken, trackIds, outputDirectory.FullName, serviceProvider),
            trackIdsOption, outputDirectoryOption, newTokenOption);

        // Playlist command options.
        var playlistIdOption = new Option<string>("--playlist-id", "The id of the playlist to download.")
        {
            IsRequired = true,
        };

        var playlistCommand = new Command("playlist", "Download a playlist's tracks.")
        {
            playlistIdOption,
        };

        playlistCommand.SetHandler(
            async (playlistId, outputDirectory, newToken) => await DownloadPlaylist(newToken, playlistId, outputDirectory.FullName, serviceProvider),
            playlistIdOption, outputDirectoryOption, newTokenOption);

        // Root command.
        var rootCommand = new RootCommand("Snippet Downloader using Spotify APIs.")
        {
            trackCommand,
            playlistCommand
        };

        foreach (var option in globalOptions)
        {
            rootCommand.AddGlobalOption(option);
        }

        return rootCommand;
    }

    private static async Task DownloadTracks(
        bool newAccessToken,
        string[] trackIds,
        string outputDirectory,
        IServiceProvider serviceProvider)
    {
        var snippetDownloader = await GetSnippetDownloader(newAccessToken, serviceProvider);

        var output = new Dictionary<string, SnippetDownloadStatus>(trackIds.Length);
        foreach (var trackId in trackIds)
        {
            var (fileName, status) = await snippetDownloader.DownloadTrack(trackId);
            output[fileName] = status;
        }

        await WriteOutput(outputDirectory, output);
    }

    private static async Task DownloadPlaylist(
        bool newAccessToken,
        string playlistId,
        string outputDirectory,
        IServiceProvider serviceProvider)
    {
        var snippetDownloader = await GetSnippetDownloader(newAccessToken, serviceProvider);

        var result = await snippetDownloader.DownloadPlaylist(playlistId);

        var output = result.ToDictionary(pair => pair.FileName, pair => pair.Status);

        await WriteOutput(outputDirectory, output);
    }

    private static async Task<ISnippetDownloader> GetSnippetDownloader(
        bool newAccessToken,
        IServiceProvider serviceProvider)
    {
        var tokenAcquirer = serviceProvider.GetRequiredService<ITokenAcquirer>();
        await tokenAcquirer.EnsureTokenExists(_requiredScopes, newAccessToken);

        return serviceProvider.GetRequiredService<ISnippetDownloader>();
    }

    private static Task WriteOutput(
        string outputDirectory,
        IReadOnlyDictionary<string, SnippetDownloadStatus> output)
    {
        Directory.CreateDirectory(outputDirectory);
        var outputFilePath = Path.Combine(outputDirectory, "output.json");

        if (File.Exists(outputFilePath))
        {
            File.Delete(outputFilePath);
        }

        using var fs = File.OpenWrite(outputFilePath);

        return JsonSerializer.SerializeAsync(fs, output, _jsonSerializerOptions);
    }
}
