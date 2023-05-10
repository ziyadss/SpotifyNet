using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.Services.Interfaces.WebAPI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.WebAPI;

public class AlbumsService : IAlbumsService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebAPIRepository _webAPIRepository;

    public AlbumsService(
        IAuthorizationService authorizationService,
        IWebAPIRepository webAPIRepository)
    {
        _authorizationService = authorizationService;
        _webAPIRepository = webAPIRepository;
    }

    public async Task<Album> GetAlbum(
        string albumId,
        CancellationToken cancellationToken)
    {
        var requiredScopes = Array.Empty<string>();

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var album = await _webAPIRepository.GetAlbum(albumId, accessToken, cancellationToken);

        return album;
    }

    public async Task<IReadOnlyList<Album>> GetAlbums(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken)
    {
        var requiredScopes = Array.Empty<string>();

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        // TODO: Chunk.
        var albums = await _webAPIRepository.GetAlbums(albumIds, accessToken, cancellationToken);

        return albums;
    }


    public async Task<IReadOnlyList<SimplifiedTrack>> GetTracks(
        string albumId,
        CancellationToken cancellationToken)
    {
        var requiredScopes = Array.Empty<string>();

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var tracks = await _webAPIRepository.GetAlbumTracks(albumId, accessToken, cancellationToken);

        return tracks;
    }

    public async Task<IReadOnlyList<SavedAlbum>> GetUserSavedAlbums(
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserLibraryRead };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var albums = await _webAPIRepository.GetSavedAlbums(accessToken, cancellationToken);

        return albums;
    }

    public async Task SaveAlbums(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserLibraryModify };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        await _webAPIRepository.SaveAlbums(albumIds, accessToken, cancellationToken);
    }

    public async Task UnsaveAlbums(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserLibraryModify };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        await _webAPIRepository.SaveAlbums(albumIds, accessToken, cancellationToken);
    }

    public async Task<IReadOnlyList<bool>> AreAlbumsSaved(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserLibraryRead };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var result = await _webAPIRepository.AreAlbumsSaved(albumIds, accessToken, cancellationToken);

        return result;
    }

    public async Task<IReadOnlyList<SimplifiedAlbum>> GetNewReleases(
        CancellationToken cancellationToken)
    {
        var requiredScopes = Array.Empty<string>();

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var releases = await _webAPIRepository.GetNewReleases(accessToken, cancellationToken);

        return releases;
    }
}
