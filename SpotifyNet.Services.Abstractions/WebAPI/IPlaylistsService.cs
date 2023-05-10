using SpotifyNet.Datastructures.Spotify.Playlists;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces.WebAPI;

public interface IPlaylistsService
{
    Task<IReadOnlyList<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlaylistTrack>> GetPlaylistTracks(
        string playlistId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SimplifiedPlaylist>> GetUserPlaylists(
        string userId,
        CancellationToken cancellationToken = default);
}
