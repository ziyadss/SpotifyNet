using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Tracks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.WebAPI;

public partial class WebAPIRepository
{
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
}
