using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
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
}
