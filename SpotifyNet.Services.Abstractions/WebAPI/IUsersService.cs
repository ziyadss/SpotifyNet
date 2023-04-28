using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces.WebAPI;

public interface IUsersService
{
    Task<IEnumerable<Track>> GetCurrentUserTopTracks(
        string timeRange = "medium_term",
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Artist>> GetCurrentUserTopArtists(
        string timeRange = "medium_term",
        CancellationToken cancellationToken = default);
}
