using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Repositories.Abstractions;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

public class TracksService : ITracksService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebAPIRepository _webAPIRepository;

    public TracksService(IAuthorizationService authorizationService, IWebAPIRepository webAPIRepository)
    {
        _authorizationService = authorizationService;
        _webAPIRepository = webAPIRepository;
    }

    public async Task<IReadOnlyList<SavedTrack>> GetCurrentUserSavedTracks(CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserLibraryRead };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var tracks = await _webAPIRepository.GetCurrentUserSavedTracks(accessToken, cancellationToken);

        return tracks;
    }


    public async Task<Track> GetTrack(string trackId, CancellationToken cancellationToken)
    {
        var accessToken = await _authorizationService.GetAccessToken(cancellationToken);

        var tracks = await _webAPIRepository.GetTrack(trackId, accessToken, cancellationToken);

        return tracks;
    }

    public async Task<IReadOnlyList<bool>> AreTracksSaved(
        IEnumerable<string> trackIds,
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserLibraryRead };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var trackIdsCollection = trackIds.ToCollection();

        var result = await trackIdsCollection
                          .ChunkedSelect(
                               chunkSize: 50,
                               chunk => _webAPIRepository.AreTracksSaved(chunk, accessToken, cancellationToken));

        return result;
    }
}
