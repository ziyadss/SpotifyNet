using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
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

    public static Task Main(
        string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services => services.AddRegistrations());

        var host = builder.Build();

        var tokenAcquirerService = host.Services.GetRequiredService<ITokenAcquirerService>();
        var snippetDownloader = host.Services.GetRequiredService<ISnippetDownloader>();

        var rootCommand = GetRootCommand(tokenAcquirerService, snippetDownloader);

        return rootCommand.InvokeAsync(args);
    }

    private static RootCommand GetRootCommand(
        ITokenAcquirerService tokenAcquirerService,
        ISnippetDownloader snippetDownloader)
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
            async (trackIds, outputDirectory, newToken) => await DownloadTracks(newToken, trackIds, outputDirectory.FullName, tokenAcquirerService, snippetDownloader),
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
            async (playlistId, outputDirectory, newToken) => await DownloadPlaylist(newToken, playlistId, outputDirectory.FullName, tokenAcquirerService, snippetDownloader),
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
        ITokenAcquirerService tokenAcquirerService,
        ISnippetDownloader snippetDownloader)
    {
        await tokenAcquirerService.EnsureTokenExists(_requiredScopes, newAccessToken);

        var tracks = new List<SnippetDownloadMetadata>(trackIds.Length);
        foreach (var trackId in trackIds)
        {
            var track = await snippetDownloader.DownloadTrack(trackId);
            tracks.Add(track);
        }

        await WriteOutput(outputDirectory, tracks);
    }

    private static async Task DownloadPlaylist(
        bool newAccessToken,
        string playlistId,
        string outputDirectory,
        ITokenAcquirerService tokenAcquirerService,
        ISnippetDownloader snippetDownloader)
    {
        await tokenAcquirerService.EnsureTokenExists(_requiredScopes, newAccessToken);

        var tracks = await snippetDownloader.DownloadPlaylist(playlistId);

        await WriteOutput(outputDirectory, tracks);
    }

    internal static SnippetDownloadStatus GetBetterStatus(SnippetDownloadStatus first, SnippetDownloadStatus second) => first switch
    {
        SnippetDownloadStatus.Unknown or SnippetDownloadStatus.NoPreviewUrl or SnippetDownloadStatus.Failed => second,
        SnippetDownloadStatus.Downloaded or SnippetDownloadStatus.Exists => first,
        _ => throw new NotImplementedException(),
    };

    private static async Task WriteOutput(
        string outputDirectory,
        IEnumerable<SnippetDownloadMetadata> output)
    {
        Directory.CreateDirectory(outputDirectory);

        var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        var outputFilePath = Path.Combine(outputDirectory, $"output-{timestamp}.json");

        if (File.Exists(outputFilePath))
        {
            File.Delete(outputFilePath);
        }

        using var fs = File.OpenWrite(outputFilePath);

        await JsonSerializer.SerializeAsync(fs, output, _jsonSerializerOptions);

        Console.WriteLine(outputFilePath);
    }
}
