using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.SnippetDownloader;

internal class SnippetDownloader : ISnippetDownloader
{
    private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars();

    private readonly string _outputDirectory;

    private readonly IWebAPIService _webAPIService;
    private readonly HttpClient _httpClient;

    public SnippetDownloader(
        string outputDirectory,
        IWebAPIService webAPIService,
        HttpClient httpClient)
    {
        _outputDirectory = outputDirectory;

        _webAPIService = webAPIService;
        _httpClient = httpClient;

        if (!string.IsNullOrEmpty(_outputDirectory))
        {
            Directory.CreateDirectory(_outputDirectory);
        }
    }

    public async Task<SnippetDownloadMetadata> DownloadTrack(
        string trackId,
        CancellationToken cancellationToken)
    {
        var track = await _webAPIService.Tracks.GetTrack(trackId, cancellationToken);

        var (fileName, status) = await DownloadTrack(track, cancellationToken);

        return new SnippetDownloadMetadata
        {
            TrackId = trackId,
            FileName = fileName,
            Status = status,
        };
    }

    public async Task<IReadOnlyList<SnippetDownloadMetadata>> DownloadPlaylist(
        string playlistId,
        CancellationToken cancellationToken)
    {
        var playlistTracks = await _webAPIService.Playlists.GetPlaylistTracks(playlistId, cancellationToken);

        var result = new List<SnippetDownloadMetadata>(playlistTracks.Count);
        foreach (var playlistTrack in playlistTracks)
        {
            var (fileName, status) = await DownloadTrack(playlistTrack.Track!, cancellationToken);

            var trackResult = new SnippetDownloadMetadata
            {
                TrackId = playlistTrack.Track!.Id!,
                FileName = fileName,
                Status = status,
            };

            result.Add(trackResult);
        }

        return result;
    }

    private async Task<(string, SnippetDownloadStatus)> DownloadTrack(
        Track track,
        CancellationToken cancellationToken)
    {
        var fileName = GetFileName(track);

        var filePath = Path.Combine(_outputDirectory, fileName);

        if (track.IsLocal == true)
        {
            return (fileName, SnippetDownloadStatus.LocalFile);
        }

        if (string.IsNullOrWhiteSpace(track.PreviewUrl))
        {
            return (fileName, SnippetDownloadStatus.NoPreviewUrl);
        }

        if (File.Exists(filePath))
        {
            return (fileName, SnippetDownloadStatus.Exists);
        }

        try
        {
            using var response = await _httpClient.GetAsync(track.PreviewUrl, cancellationToken);
            await Ensure.RequestSuccess(response, cancellationToken);

            using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await responseStream.CopyToAsync(fileStream, cancellationToken);

            return (fileName, SnippetDownloadStatus.Downloaded);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
            return (fileName, SnippetDownloadStatus.Failed);
        }
    }

    private static string GetFileName(Track track)
    {
        var trackName = track.Name;

        var artists = track.Artists?.Select(a => a.Name) ?? Array.Empty<string>();
        var artistsNames = string.Join(", ", artists);

        var originalFileName = $"{trackName} - {artistsNames}.mp3";

        var validSections = originalFileName.Split(_invalidFileNameChars, StringSplitOptions.RemoveEmptyEntries);
        var fileName = string.Join('_', validSections);

        return fileName;
    }
}
