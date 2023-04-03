using SpotifyNet.Datastructures.Spotify.Playlists;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SpotifyNet.WebAPI.Interfaces;

public interface IWebAPIRepository
{
    Task<IReadOnlyList<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        string accessToken,
        string? ownerId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlaylistTrack>> GetPlaylistItems(
        string accessToken,
        string playlistId,
        CancellationToken cancellationToken = default);
}
