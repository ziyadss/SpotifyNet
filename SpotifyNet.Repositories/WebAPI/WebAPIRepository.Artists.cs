using SpotifyNet.Datastructures.Spotify.Artists;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    public Task<Artist> GetArtist(
        string artistId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetArtist(artistId);

        return _webAPIClient.GetAsync<Artist>(
            uri,
            accessToken,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Artist>> GetArtists(
        IEnumerable<string> artistIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetSeveralArtists(artistIds);

        var artists = await _webAPIClient.GetAsync<ArtistsSet>(
            uri,
            accessToken,
            cancellationToken);

        return artists.Artists;
    }
}
