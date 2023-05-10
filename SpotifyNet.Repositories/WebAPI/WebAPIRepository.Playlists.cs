using SpotifyNet.Datastructures.Spotify.Playlists;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    public Task<IReadOnlyList<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetCurrentUserPlaylists();

        return GetOffsetPaginated<SimplifiedPlaylist>(
            uri,
            accessToken,
            cancellationToken);
    }

    public Task<IReadOnlyList<PlaylistTrack>> GetPlaylistItems(
        string playlistId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetPlaylistItems(playlistId);

        return GetOffsetPaginated<PlaylistTrack>(
            uri,
            accessToken,
            cancellationToken);
    }

    public Task<IReadOnlyList<SimplifiedPlaylist>> GetUserPlaylists(
        string userId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetUserPlaylists(userId);

        return GetOffsetPaginated<SimplifiedPlaylist>(
            uri,
            accessToken,
            cancellationToken);
    }
}
