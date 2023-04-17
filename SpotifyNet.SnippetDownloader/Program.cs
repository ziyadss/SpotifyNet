using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Clients;
using SpotifyNet.Common;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Repositories;
using SpotifyNet.Services;
using SpotifyNet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyNet.Playground;

sealed internal class Program
{
    private const string AppClientId = "";
    private const string AppRedirectUri = "http://localhost:3000";

    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services =>
        {
            services
            .AddAuthorizationClient(AppClientId, AppRedirectUri)
            .AddAuthorizationRepository()
            .AddAuthorizationService()
            .AddWebAPIClient()
            .AddWebAPIRepository()
            .AddWebAPIService()

            .AddRedirectUriListener(AppRedirectUri)
            .AddTokenAcquirer();
        });

        var host = builder.Build();

        await Test(host.Services);
    }

    enum TrackDownloadStatus
    {
        Unknown = 0,
        Downloaded,
        NoPreviewUrl,
        Failed,
        Exists,
    };

    private static async Task Test(IServiceProvider serviceProvider)
    {
        var newToken = false;
        var scopes = new[] { AuthorizationScope.UserLibraryRead, AuthorizationScope.PlaylistReadPrivate };

        if (newToken)
        {
            var tokenAcquirer = serviceProvider.GetRequiredService<ITokenAcquirer>();
            await tokenAcquirer.GenerateToken(scopes);
        }

        var webAPIService = serviceProvider.GetRequiredService<IWebAPIService>();
        using var httpClient = new HttpClient();
        var playlistTracks = await webAPIService.GetPlaylistTracks("5aMKlx7LBLKPLUVukF6X0M");
        playlistTracks = playlistTracks.Take(1);

        var output = new Dictionary<string, TrackDownloadStatus>();

        foreach (var playlistTrack in playlistTracks)
        {
            var track = playlistTrack.Track!;

            var trackName = track.Name;
            var artistsNames = string.Join(", ", track.Artists!.Select(a => a.Name));

            var fileName = $"{trackName} - {artistsNames}.mp3";
            var downloadStatus = TrackDownloadStatus.Unknown;
            try
            {
                if (File.Exists(fileName))
                {
                    downloadStatus = TrackDownloadStatus.Exists;
                }
                else if (string.IsNullOrWhiteSpace(track.PreviewUrl))
                {
                    downloadStatus = TrackDownloadStatus.NoPreviewUrl;
                }
                else
                {
                    var response = await httpClient.GetAsync(track.PreviewUrl);
                    await Ensure.RequestSuccess(response);

                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    using var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    await responseStream.CopyToAsync(fileStream);

                    downloadStatus = TrackDownloadStatus.Downloaded;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                downloadStatus = TrackDownloadStatus.Failed;
            }
            finally
            {
                output[fileName] = downloadStatus;
            }
        }
    }

    private static async Task Write<T>(string fileName, T item)
    {
        File.Delete(fileName);
        using var fs = File.OpenWrite(fileName);
        await JsonSerializer.SerializeAsync(fs, item);
    }
}
