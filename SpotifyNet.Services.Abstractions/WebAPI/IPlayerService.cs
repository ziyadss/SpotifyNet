using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Player;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IPlayerService
{
    Task SetPlaybackVolume(int volume, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlayHistory>> GetRecentlyPlayedTracks(CancellationToken cancellationToken = default);
}
