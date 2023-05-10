using SpotifyNet.Datastructures.Spotify.Player;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    public Task<IReadOnlyList<PlayHistory>> GetRecentlyPlayedTracks(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetRecentlyPlayedTracks();

        return GetCursorPaginated<PlayHistory>(
            uri,
            accessToken,
            cancellationToken);
    }
}
