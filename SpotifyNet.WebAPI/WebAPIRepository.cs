using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Tracks.Analysis;
using SpotifyNet.WebAPI.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.WebAPI;

public class WebAPIRepository : IWebAPIRepository
{
    private readonly IWebAPIClient _webAPIClient;

    public WebAPIRepository(IWebAPIClient webAPIClient)
    {
        _webAPIClient = webAPIClient;
    }

    // Albums
    public Task<Album> GetAlbum(
        string albumId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetAlbum(albumId);

        return _webAPIClient.GetAsync<Album>(
            url,
            accessToken,
            cancellationToken);
    }

    public async Task<IEnumerable<Album>> GetAlbums(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetSeveralAlbums(albumIds);

        var albums = await _webAPIClient.GetAsync<AlbumsSet>(
            url,
            accessToken,
            cancellationToken);

        return albums.Albums;
    }

    public Task<IEnumerable<SimplifiedTrack>> GetAlbumTracks(
        string albumId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetAlbumTracks(albumId);

        return GetPaginated<SimplifiedTrack>(
            url,
            accessToken,
            cancellationToken);
    }

    public Task<IEnumerable<SavedAlbum>> GetSavedAlbums(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetUserSavedAlbums();

        return GetPaginated<SavedAlbum>(
            url,
            accessToken,
            cancellationToken);
    }

    public Task SaveAlbums(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        Ensure.Between(albumIds.Length, 0, 50, inclusive: true);

        var url = Endpoints.SaveAlbumsForUser();

        var payload = new
        {
            ids = albumIds,
        };

        return _webAPIClient.PutAsync(
            url,
            payload,
            accessToken,
            cancellationToken);
    }

    public Task RemoveAlbums(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        Ensure.Between(albumIds.Length, 0, 50, inclusive: true);

        var url = Endpoints.RemoveUserSavedAlbums();

        var payload = new
        {
            ids = albumIds,
        };

        return _webAPIClient.DeleteAsync(
            url,
            payload,
            accessToken,
            cancellationToken);
    }

    public Task<IEnumerable<bool>> AreAlbumsSaved(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        Ensure.Between(albumIds.Length, 0, 20, inclusive: true);

        var url = Endpoints.CheckUserSavedAlbums(albumIds);

        return _webAPIClient.GetAsync<IEnumerable<bool>>(
            url,
            accessToken,
            cancellationToken);
    }

    public Task<IEnumerable<SimplifiedAlbum>> GetNewReleases(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetNewReleases();

        return GetPaginated<SimplifiedAlbum>(
            url,
            accessToken,
            cancellationToken);
    }

    // Artists

    // Audiobooks

    // Categories

    // Chapters

    // Episodes

    // Genres

    // Markets

    // Player

    // Playlists
    public Task<IEnumerable<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetCurrentUserPlaylists();

        return GetPaginated<SimplifiedPlaylist>(
            url,
            accessToken,
            cancellationToken);
    }

    public Task<IEnumerable<PlaylistTrack>> GetPlaylistItems(
        string playlistId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetPlaylistItems(playlistId);

        return GetPaginated<PlaylistTrack>(
            url,
            accessToken,
            cancellationToken);
    }

    // Search

    // Shows

    // Tracks
    public async Task<IEnumerable<Track>> GetTracks(
        string[] trackIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        Ensure.Between(trackIds.Length, 0, 50, inclusive: true);

        var url = Endpoints.GetSeveralTracks(trackIds);

        var tracks = await _webAPIClient.GetAsync<TracksSet>(
            url,
            accessToken,
            cancellationToken);

        return tracks.Tracks;
    }

    public Task<IEnumerable<SavedTrack>> GetCurrentUserSavedTracks(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetUserSavedTracks();

        return GetPaginated<SavedTrack>(
            url,
            accessToken,
            cancellationToken);
    }

    public async Task<IEnumerable<AudioFeatures>> GetTracksAudioFeatures(
        string[] trackIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        Ensure.Between(trackIds.Length, 0, 100, inclusive: true);

        var url = Endpoints.GetTracksAudioFeatures(trackIds);

        var features = await _webAPIClient.GetAsync<AudioFeaturesSet>(
            url,
            accessToken,
            cancellationToken);

        return features.AudioFeatures;
    }

    public async Task<AudioAnalysis> GetAudioAnalysis(
        string trackId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetTrackAudioAnalysis(trackId);

        return await _webAPIClient.GetAsync<AudioAnalysis>(
            url,
            accessToken,
            cancellationToken);
    }

    // Users

    private async Task<IEnumerable<T>> GetPaginated<T>(
        string initialUrl,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var batch = await _webAPIClient.GetAsync<PaginationWrapper<T>>(
            initialUrl,
            accessToken,
            cancellationToken);

        var items = new List<T>(batch.Total);
        items.AddRange(batch.Items);

        while (batch.Next is not null)
        {
            batch = await _webAPIClient.GetAsync<PaginationWrapper<T>>(
                batch.Next,
                accessToken,
                cancellationToken);

            items.AddRange(batch.Items);
        }

        return items;
    }
}
