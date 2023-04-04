using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Tracks.Analysis;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.Interfaces;

public interface IWebAPIRepository
{
    // Albums
    Task<Album> GetAlbum(
        string albumId,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Album>> GetAlbums(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SimplifiedTrack>> GetAlbumTracks(
        string albumId,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SavedAlbum>> GetSavedAlbums(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task SaveAlbums(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task RemoveAlbums(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<bool>> AreAlbumsSaved(
        string[] albumIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SimplifiedAlbum>> GetNewReleases(
        string accessToken,
        CancellationToken cancellationToken = default);

    // Artists

    // Audiobooks

    // Categories

    // Chapters

    // Episodes

    // Genres

    // Markets

    // Player

    // Playlists
    Task<IEnumerable<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<PlaylistTrack>> GetPlaylistItems(
        string playlistId,
        string accessToken,
        CancellationToken cancellationToken = default);

    // Search

    // Shows

    // Tracks
    Task<IEnumerable<Track>> GetTracks(
    string[] trackIds,
    string accessToken,
    CancellationToken cancellationToken = default);

    Task<IEnumerable<SavedTrack>> GetCurrentUserSavedTracks(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<AudioFeatures>> GetTracksAudioFeatures(
        string[] trackIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<AudioFeatures> GetTrackAudioFeatures(
        string trackId,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<AudioAnalysis> GetAudioAnalysis(
        string trackId,
        string accessToken,
        CancellationToken cancellationToken = default);

    // Users
}
