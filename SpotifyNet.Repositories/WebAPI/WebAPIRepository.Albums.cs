﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    public Task<Album> GetAlbum(string albumId, string accessToken, CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetAlbum(albumId);

        return _webAPIClient.GetAsync<Album>(uri, accessToken, cancellationToken);
    }

    public async Task<IReadOnlyList<Album>> GetAlbums(
        IEnumerable<string> albumIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetSeveralAlbums(albumIds);

        var albums = await _webAPIClient.GetAsync<AlbumsSet>(uri, accessToken, cancellationToken);

        return albums.Albums;
    }

    public Task<IReadOnlyList<SimplifiedTrack>> GetAlbumTracks(
        string albumId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetAlbumTracks(albumId);

        return GetOffsetPaginated<SimplifiedTrack>(uri, accessToken, cancellationToken);
    }

    public Task<IReadOnlyList<SavedAlbum>> GetSavedAlbums(string accessToken, CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetUserSavedAlbums();

        return GetOffsetPaginated<SavedAlbum>(uri, accessToken, cancellationToken);
    }

    public Task SaveAlbums(IEnumerable<string> albumIds, string accessToken, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(albumIds, nameof(albumIds));

        var albumIdsCollection = albumIds.ToCollection();
        Ensure.Between(albumIdsCollection.Count, 1, 50, inclusive: true);

        var uri = Endpoints.SaveAlbumsForUser();

        var payload = new
        {
            ids = albumIdsCollection,
        };

        return _webAPIClient.PutAsync(uri, payload, accessToken, cancellationToken);
    }

    public Task RemoveAlbums(IEnumerable<string> albumIds, string accessToken, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(albumIds, nameof(albumIds));

        var albumIdsCollection = albumIds.ToCollection();
        Ensure.Between(albumIdsCollection.Count, 1, 50, inclusive: true);

        var uri = Endpoints.RemoveUserSavedAlbums();

        var payload = new
        {
            ids = albumIdsCollection,
        };

        return _webAPIClient.DeleteAsync(uri, payload, accessToken, cancellationToken);
    }

    public Task<IReadOnlyList<bool>> AreAlbumsSaved(
        IEnumerable<string> albumIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(albumIds, nameof(albumIds));

        var albumIdsCollection = albumIds.ToCollection();
        Ensure.Between(albumIdsCollection.Count, 1, 20, inclusive: true);

        var uri = Endpoints.CheckUserSavedAlbums(albumIdsCollection);

        return _webAPIClient.GetAsync<IReadOnlyList<bool>>(uri, accessToken, cancellationToken);
    }

    public Task<IReadOnlyList<SimplifiedAlbum>> GetNewReleases(string accessToken, CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetNewReleases();

        return GetOffsetPaginated<SimplifiedAlbum>(uri, accessToken, cancellationToken);
    }
}
