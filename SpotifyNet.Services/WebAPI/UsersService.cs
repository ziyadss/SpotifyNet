using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.Services.Interfaces.WebAPI;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.WebAPI;

public class UsersService : IUsersService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebAPIRepository _webAPIRepository;

    public UsersService(
        IAuthorizationService authorizationService,
        IWebAPIRepository webAPIRepository)
    {
        _authorizationService = authorizationService;
        _webAPIRepository = webAPIRepository;
    }

    public async Task<IEnumerable<Track>> GetCurrentUserTopTracks(
        string timeRange,
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserTopRead };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var tracks = await _webAPIRepository.GetCurrentUserTopTracks(timeRange, accessToken, cancellationToken);

        return tracks;
    }

    public async Task<IEnumerable<Artist>> GetCurrentUserTopArtists(
        string timeRange,
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserTopRead };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var artists = await _webAPIRepository.GetCurrentUserTopArtists(timeRange, accessToken, cancellationToken);

        return artists;
    }
}
