using SpotifyNet.Datastructures.Spotify.Player;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces.WebAPI;

public interface IPlayerService
{
    Task<IReadOnlyList<PlayHistory>> GetRecentlyPlayedTracks(
        CancellationToken cancellationToken = default);
}
