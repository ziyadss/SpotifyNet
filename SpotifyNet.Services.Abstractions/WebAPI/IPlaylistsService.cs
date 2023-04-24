using SpotifyNet.Datastructures.Spotify.Playlists;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces.WebAPI;

public interface IPlaylistsService
{
    Task<IEnumerable<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        CancellationToken cancellationToken = default);

    Task<IEnumerable<PlaylistTrack>> GetPlaylistTracks(
        string playlistId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SimplifiedPlaylist>> GetUserPlaylists(
        string userId,
        CancellationToken cancellationToken = default);
}
