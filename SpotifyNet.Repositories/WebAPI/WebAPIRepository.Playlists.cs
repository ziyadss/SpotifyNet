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
        var uri = Endpoints.GetCurrentUserPlaylists();

        return GetPaginated<SimplifiedPlaylist>(
            uri,
            accessToken,
            cancellationToken);
    }

    public Task<IEnumerable<PlaylistTrack>> GetPlaylistItems(
        string playlistId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetPlaylistItems(playlistId);

        return GetPaginated<PlaylistTrack>(
            uri,
            accessToken,
            cancellationToken);
    }
}
