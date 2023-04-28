using SpotifyNet.Datastructures.Spotify.Player;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces.WebAPI;

public interface IPlayerService
{
    Task<IEnumerable<PlayHistory>> GetRecentlyPlayedTracks(
        CancellationToken cancellationToken = default);
}
