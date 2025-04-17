using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Users;
using SpotifyNet.Repositories.Abstractions;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

public class UsersService : IUsersService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebAPIRepository _webAPIRepository;

    public UsersService(IAuthorizationService authorizationService, IWebAPIRepository webAPIRepository)
    {
        _authorizationService = authorizationService;
        _webAPIRepository = webAPIRepository;
    }

    public async Task<User> GetCurrentUserProfile(CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserReadPrivate, AuthorizationScope.UserReadEmail };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var user = await _webAPIRepository.GetCurrentUserProfile(accessToken, cancellationToken);

        return user;
    }

    public async Task<IReadOnlyList<Track>> GetCurrentUserTopTracks(
        string timeRange,
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserTopRead };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var tracks = await _webAPIRepository.GetCurrentUserTopTracks(timeRange, accessToken, cancellationToken);

        return tracks;
    }

    public async Task<IReadOnlyList<Artist>> GetCurrentUserTopArtists(
        string timeRange,
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserTopRead };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var artists = await _webAPIRepository.GetCurrentUserTopArtists(timeRange, accessToken, cancellationToken);

        return artists;
    }

    public async Task<User> GetUserProfile(string userId, CancellationToken cancellationToken)
    {
        var accessToken = await _authorizationService.GetAccessToken(cancellationToken);

        var user = await _webAPIRepository.GetUserProfile(userId, accessToken, cancellationToken);

        return user;
    }
}
