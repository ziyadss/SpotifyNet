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

    public async Task<SnippetDownloadMetadata> DownloadTrack(
        string trackId,
        CancellationToken cancellationToken)
    {
        var track = await _webAPIService.Tracks.GetTrack(trackId, cancellationToken);

        var (fileName, status) = await DownloadTrack(track, cancellationToken);

        var result = new SnippetDownloadMetadata
        {
            TrackId = trackId,
            FileName = fileName,
            Status = status,
        };

        if (status is SnippetDownloadStatus.Downloaded or SnippetDownloadStatus.Exists)
        {
            var artistId = track.Artists!.First().Id!;
            var artist = await _webAPIService.Artists.GetArtist(artistId, cancellationToken);

            result.Genres = artist.Genres;
        }

        return result;
    }

    public async Task<IEnumerable<SnippetDownloadMetadata>> DownloadPlaylist(
        string playlistId,
        CancellationToken cancellationToken)
    {
        var playlistTracks = await _webAPIService.Playlists.GetPlaylistTracks(playlistId, cancellationToken);

        var artistToTracks = new Dictionary<string, List<SnippetDownloadMetadata>>();
        var failedTracks = new List<SnippetDownloadMetadata>();
        foreach (var playlistTrack in playlistTracks)
        {
            var (fileName, status) = await DownloadTrack(playlistTrack.Track!, cancellationToken);

            var trackResult = new SnippetDownloadMetadata
            {
                TrackId = playlistTrack.Track!.Id!,
                FileName = fileName,
                Status = status,
            };

            if (status is SnippetDownloadStatus.Downloaded or SnippetDownloadStatus.Exists)
            {
                var artistId = playlistTrack.Track!.Artists!.First().Id!;

                if (artistToTracks.TryGetValue(artistId, out var tracks))
                {
                    tracks.Add(trackResult);
                }
                else
                {
                    artistToTracks[artistId] = new List<SnippetDownloadMetadata> { trackResult };
                }
            }
            else
            {
                failedTracks.Add(trackResult);
            }
        }

        var artists = await _webAPIService.Artists.GetArtists(artistToTracks.Keys, cancellationToken);
        var idToGenres = artists.ToDictionary(a => a.Id!, a => a.Genres);

        foreach (var (artistId, tracks) in artistToTracks)
        {
            var genres = idToGenres[artistId];
            foreach (var track in tracks)
            {
                track.Genres = genres;
            }
        }

        var result = artistToTracks
            .SelectMany(kvp => kvp.Value)
            .Concat(failedTracks);

        return result;
    }

    public async Task<(string, SnippetDownloadStatus)> DownloadTrack(
        Track track,
        CancellationToken cancellationToken)
    {
        var fileName = GetFileName(track);
        var validSections = fileName.Split(_invalidFileNameChars, StringSplitOptions.RemoveEmptyEntries);
        fileName = string.Join('_', validSections);

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
        var artistsNames = string.Join(", ", track.Artists?.Select(a => a.Name) ?? Array.Empty<string>());
        return $"{trackName} - {artistsNames}.mp3";
    }
}
