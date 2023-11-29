using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Artists;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IArtistsService
{
    Task<Artist> GetArtist(string artistId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Artist>> GetArtists(
        IEnumerable<string> artistIds,
        CancellationToken cancellationToken = default);
}
