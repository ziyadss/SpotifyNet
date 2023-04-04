using SpotifyNet.Datastructures.Spotify.Playlists;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    public Task<IEnumerable<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetCurrentUserPlaylists();

        return GetPaginated<SimplifiedPlaylist>(
            url,
            accessToken,
            cancellationToken);
    }

    public Task<IEnumerable<PlaylistTrack>> GetPlaylistItems(
        string playlistId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetPlaylistItems(playlistId);

        return GetPaginated<PlaylistTrack>(
            url,
            accessToken,
            cancellationToken);
    }
}
