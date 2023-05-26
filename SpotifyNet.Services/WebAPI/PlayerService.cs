using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Player;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.Services.Interfaces.WebAPI;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.WebAPI;

public class PlayerService : IPlayerService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebAPIRepository _webAPIRepository;

    public PlayerService(
        IAuthorizationService authorizationService,
        IWebAPIRepository webAPIRepository)
    {
        _authorizationService = authorizationService;
        _webAPIRepository = webAPIRepository;
    }

    public async Task SetPlaybackVolume(
        int volume,
        CancellationToken cancellationToken)
    {
        // TODO: Allow for setting volume for specific device.
        Ensure.Between(volume, 0, 100, inclusive: true);

        var requiredScopes = new[] { AuthorizationScope.UserModifyPlaybackState };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        await _webAPIRepository.SetPlaybackVolume(volume, accessToken, cancellationToken);
    }

    public async Task<IReadOnlyList<PlayHistory>> GetRecentlyPlayedTracks(
        CancellationToken cancellationToken)
    {
        var requiredScopes = new[] { AuthorizationScope.UserReadRecentlyPlayed };

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var history = await _webAPIRepository.GetRecentlyPlayedTracks(accessToken, cancellationToken);

        return history;
    }
}
