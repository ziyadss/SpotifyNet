using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.Services.Interfaces.WebAPI;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.WebAPI;

public class PlaylistsService : IPlaylistsService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebAPIRepository _webAPIRepository;

    public PlaylistsService(
        IAuthorizationService authorizationService,
        IWebAPIRepository webAPIRepository)
    {
        _authorizationService = authorizationService;
        _webAPIRepository = webAPIRepository;
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
}
