using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Player;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    public Task SetPlaybackVolume(int volume, string accessToken, CancellationToken cancellationToken)
    {
        var uri = Endpoints.SetPlaybackVolume(volume);

        return _webAPIClient.PutAsync(uri, accessToken, cancellationToken);
    }

    public Task<IReadOnlyList<PlayHistory>> GetRecentlyPlayedTracks(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetRecentlyPlayedTracks();

        return GetCursorPaginated<PlayHistory>(uri, accessToken, cancellationToken);
    }
}
