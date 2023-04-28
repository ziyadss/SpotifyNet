using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    public Task<IEnumerable<Track>> GetCurrentUserTopTracks(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetCurrentUserTopItems("tracks");

        return GetOffsetPaginated<Track>(
            uri,
            accessToken,
            cancellationToken);
    }

    public Task<IEnumerable<Artist>> GetCurrentUserTopArtists(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetCurrentUserTopItems("artists");

        return GetOffsetPaginated<Artist>(
            uri,
            accessToken,
            cancellationToken);
    }
}
