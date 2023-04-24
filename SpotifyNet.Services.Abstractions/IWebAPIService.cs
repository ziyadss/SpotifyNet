using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces;

public interface IWebAPIService
{
    Task<IEnumerable<SavedTrack>> GetCurrentUserSavedTracks(
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        CancellationToken cancellationToken = default);

    Task<IEnumerable<PlaylistTrack>> GetPlaylistTracks(
        string playlistId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SimplifiedPlaylist>> GetUserPlaylists(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Track> GetTrack(
        string trackId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Track>> GetCurrentUserTopTracks(
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Artist>> GetCurrentUserTopArtists(
        CancellationToken cancellationToken = default);

    Task<Album> GetAlbum(
        string albumId,
        CancellationToken cancellationToken = default);
}
