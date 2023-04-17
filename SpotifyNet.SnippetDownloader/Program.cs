using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Common;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyNet.SnippetDownloader;

sealed internal class Program
{
    private static string _outputDirectory = string.Empty;

    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services => services.AddRegistrations());

        var host = builder.Build();

        await Run(host.Services);
    }

    enum TrackDownloadStatus
    {
        Unknown = 0,
        Downloaded,
        NoPreviewUrl,
        Failed,
        Exists,
    };

    private static async Task Run(IServiceProvider serviceProvider)
    {
        var newToken = true;
        var scopes = new[] { AuthorizationScope.PlaylistReadPrivate };

        if (newToken)
        {
            var tokenAcquirer = serviceProvider.GetRequiredService<ITokenAcquirer>();
            await tokenAcquirer.GenerateToken(scopes);
        }

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        _outputDirectory = configuration["outputDirectory"]!;

        var webAPIService = serviceProvider.GetRequiredService<IWebAPIService>();

        var playlistTracks = await webAPIService.GetPlaylistTracks("5aMKlx7LBLKPLUVukF6X0M");
        playlistTracks = playlistTracks.Take(1);

        var output = new Dictionary<string, TrackDownloadStatus>();

        using var httpClient = new HttpClient();

        foreach (var playlistTrack in playlistTracks)
        {
            var (fileName, downloadStatus) = await DownloadTrack(httpClient, playlistTrack);
            output[fileName] = downloadStatus;
        }

        var outputFilePath = Path.Combine(_outputDirectory, "output.json");
        await Write(outputFilePath, output);
    }

    private static async Task<(string, TrackDownloadStatus)> DownloadTrack(HttpClient httpClient, PlaylistTrack playlistTrack)
    {
        var track = playlistTrack.Track!;

        var trackName = track.Name;
        var artistsNames = string.Join(", ", track.Artists!.Select(a => a.Name));

        var fileName = $"{trackName} - {artistsNames}.mp3";
        var filePath = Path.Combine(_outputDirectory, fileName);

        try
        {
            if (File.Exists(filePath))
            {
                return (fileName, TrackDownloadStatus.Exists);
            }
            else if (string.IsNullOrWhiteSpace(track.PreviewUrl))
            {
                return (fileName, TrackDownloadStatus.NoPreviewUrl);
            }
            else
            {
                var response = await httpClient.GetAsync(track.PreviewUrl);
                await Ensure.RequestSuccess(response);

                using var responseStream = await response.Content.ReadAsStreamAsync();
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await responseStream.CopyToAsync(fileStream);

                return (fileName, TrackDownloadStatus.Downloaded);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return (fileName, TrackDownloadStatus.Failed);
        }
    }

    private static async Task Write<T>(string fileName, T item)
    {
        File.Delete(fileName);
        using var fs = File.OpenWrite(fileName);
        await JsonSerializer.SerializeAsync(fs, item);
    }
}
