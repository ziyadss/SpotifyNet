using SpotifyNet.Datastructures.Spotify.Playlists;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.WebAPI.Interfaces;

public interface IWebAPIRepository
{
    Task<IReadOnlyList<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        string accessToken,
        string? ownerId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlaylistTrack>> GetPlaylistItems(
        string playlistId, 
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SavedTrack>> GetCurrentUserSavedTracks(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AudioFeatures>> GetTracksAudioFeatures(
        string[] trackIds,
        string accessToken,
        CancellationToken cancellationToken = default);
}
