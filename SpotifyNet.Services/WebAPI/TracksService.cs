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

public class TracksService : ITracksService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebAPIRepository _webAPIRepository;

    public TracksService(
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
