using SpotifyNet.Datastructures.Spotify.Artists;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces.WebAPI;

public interface IArtistsService
{
    Task<Artist> GetArtist(
        string artistId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Artist>> GetArtists(
        IEnumerable<string> artistIds,
        CancellationToken cancellationToken = default);
}
