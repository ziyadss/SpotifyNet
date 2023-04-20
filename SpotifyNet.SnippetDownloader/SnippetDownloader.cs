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
    private readonly static char[] _invalidFileNameChars = Path.GetInvalidFileNameChars();

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

        Directory.CreateDirectory(_outputDirectory);
    }

    public async Task<(string, SnippetDownloadStatus)> DownloadTrack(
        string trackId,
        CancellationToken cancellationToken)
    {
        var track = await _webAPIService.GetTrack(trackId, cancellationToken);

        var result = await DownloadTrack(track, cancellationToken);

        return result;
    }

    public async Task<IEnumerable<(string, SnippetDownloadStatus)>> DownloadPlaylist(
        string playlistId,
        CancellationToken cancellationToken)
    {
        var playlistTracks = await _webAPIService.GetPlaylistTracks(playlistId, cancellationToken);

        var tracks = playlistTracks.Select(pt => pt.Track!);

        var result = new List<(string, SnippetDownloadStatus)>();
        foreach (var track in tracks)
        {
            var trackResult = await DownloadTrack(track, cancellationToken);
            result.Add(trackResult);
        }

        return result;
    }

    public async Task<(string, SnippetDownloadStatus)> DownloadTrack(
        Track track,
        CancellationToken cancellationToken)
    {
        var trackName = track.Name;
        var artistsNames = string.Join(", ", track.Artists!.Select(a => a.Name));

        var fileName = $"{trackName} - {artistsNames}.mp3";

        var validSections = fileName.Split(_invalidFileNameChars, StringSplitOptions.RemoveEmptyEntries);
        fileName = string.Join('_', validSections);

        var filePath = Path.Combine(_outputDirectory, fileName);

        try
        {
            if (File.Exists(filePath))
            {
                return (fileName, SnippetDownloadStatus.Exists);
            }
            else if (string.IsNullOrWhiteSpace(track.PreviewUrl))
            {
                return (fileName, SnippetDownloadStatus.NoPreviewUrl);
            }
            else
            {
                var response = await _httpClient.GetAsync(track.PreviewUrl, cancellationToken);
                await Ensure.RequestSuccess(response, cancellationToken);

                using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await responseStream.CopyToAsync(fileStream, cancellationToken);

                return (fileName, SnippetDownloadStatus.Downloaded);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return (fileName, SnippetDownloadStatus.Failed);
        }
    }
}
