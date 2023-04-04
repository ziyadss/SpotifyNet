﻿using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Tracks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    public Task<Album> GetAlbum(
        string albumId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetAlbum(albumId);

        return _webAPIClient.GetAsync<Album>(
            uri,
            accessToken,
            cancellationToken);
    }

    public async Task<IEnumerable<Album>> GetAlbums(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetSeveralAlbums(albumIds);

        var albums = await _webAPIClient.GetAsync<AlbumsSet>(
            uri,
            accessToken,
            cancellationToken);

        return albums.Albums;
    }

    public Task<IEnumerable<SimplifiedTrack>> GetAlbumTracks(
        string albumId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetAlbumTracks(albumId);

        return GetPaginated<SimplifiedTrack>(
            uri,
            accessToken,
            cancellationToken);
    }

    public Task<IEnumerable<SavedAlbum>> GetSavedAlbums(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetUserSavedAlbums();

        return GetPaginated<SavedAlbum>(
            uri,
            accessToken,
            cancellationToken);
    }

    public Task SaveAlbums(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(albumIds, nameof(albumIds));

        Ensure.Between(albumIds.Length, 0, 50, inclusive: true);

        var uri = Endpoints.SaveAlbumsForUser();

        var payload = new
        {
            ids = albumIds,
        };

        return _webAPIClient.PutAsync(
            uri,
            payload,
            accessToken,
            cancellationToken);
    }

    public Task RemoveAlbums(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(albumIds, nameof(albumIds));

        Ensure.Between(albumIds.Length, 0, 50, inclusive: true);

        var uri = Endpoints.RemoveUserSavedAlbums();

        var payload = new
        {
            ids = albumIds,
        };

        return _webAPIClient.DeleteAsync(
            uri,
            payload,
            accessToken,
            cancellationToken);
    }

    public Task<IEnumerable<bool>> AreAlbumsSaved(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(albumIds, nameof(albumIds));

        Ensure.Between(albumIds.Length, 0, 20, inclusive: true);

        var uri = Endpoints.CheckUserSavedAlbums(albumIds);

        return _webAPIClient.GetAsync<IEnumerable<bool>>(
            uri,
            accessToken,
            cancellationToken);
    }

    public Task<IEnumerable<SimplifiedAlbum>> GetNewReleases(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetNewReleases();

        return GetPaginated<SimplifiedAlbum>(
            uri,
            accessToken,
            cancellationToken);
    }
}