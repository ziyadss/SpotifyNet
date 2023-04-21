using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.WebAPI;

public class WebAPIService : IWebAPIService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebAPIRepository _webAPIRepository;

    public WebAPIService(
        IAuthorizationService authorizationService,
        IWebAPIRepository webAPIRepository)
    {
        _authorizationService = authorizationService;
        _webAPIRepository = webAPIRepository;
    }

    public async Task<IEnumerable<SavedTrack>> GetCurrentUserSavedTracks(
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserLibraryRead };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var tracks = await _webAPIRepository.GetCurrentUserSavedTracks(accessToken, cancellationToken);

        return tracks;
    }

    public async Task<IEnumerable<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.PlaylistReadPrivate };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var playlists = await _webAPIRepository.GetCurrentUserPlaylists(accessToken, cancellationToken);

        return playlists;
    }

    public async Task<IEnumerable<PlaylistTrack>> GetPlaylistTracks(
        string playlistId,
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserLibraryRead };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var tracks = await _webAPIRepository.GetPlaylistItems(playlistId, accessToken, cancellationToken);

        return tracks;
    }

    public async Task<IEnumerable<SimplifiedPlaylist>> GetUserPlaylists(
        string userId,
        CancellationToken cancellationToken)
    {
        // TODO: Are these scopes required? I think they're optional?
        var requiredScopes = new[] { AuthorizationScope.PlaylistReadPrivate, AuthorizationScope.PlaylistReadCollaborative };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var playlists = await _webAPIRepository.GetUserPlaylists(userId, accessToken, cancellationToken);

        return playlists;
    }

    public async Task<Track> GetTrack(
        string trackId,
        CancellationToken cancellationToken)
    {
        var requiredScopes = Array.Empty<string>();

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var tracks = await _webAPIRepository.GetTrack(trackId, accessToken, cancellationToken);

        return tracks;
    }
}
