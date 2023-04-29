using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Player;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Tracks.Analysis;
using SpotifyNet.Datastructures.Spotify.Users;
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
        IEnumerable<string> albumIds,
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
        IEnumerable<string> albumIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task RemoveAlbums(
        IEnumerable<string> albumIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<bool>> AreAlbumsSaved(
        IEnumerable<string> albumIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SimplifiedAlbum>> GetNewReleases(
        string accessToken,
        CancellationToken cancellationToken = default);

    // Artists
    Task<Artist> GetArtist(
        string artistId,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Artist>> GetArtists(
        IEnumerable<string> artistIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    // Audiobooks

    // Categories

    // Chapters

    // Episodes

    // Genres

    // Markets

    // Player
    Task<IEnumerable<PlayHistory>> GetRecentlyPlayedTracks(
        string accessToken,
        CancellationToken cancellationToken = default);

    // Playlists
    Task<IEnumerable<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<PlaylistTrack>> GetPlaylistItems(
        string playlistId,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SimplifiedPlaylist>> GetUserPlaylists(
        string userId,
        string accessToken,
        CancellationToken cancellationToken = default);

    // Search

    // Shows

    // Tracks
    Task<Track> GetTrack(
        string trackId,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Track>> GetTracks(
        IEnumerable<string> trackIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SavedTrack>> GetCurrentUserSavedTracks(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<AudioFeatures>> GetTracksAudioFeatures(
        IEnumerable<string> trackIds,
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
    Task<User> GetCurrentUserProfile(
        string accessToken,
        CancellationToken cancellationToken= default);

    Task<IEnumerable<Track>> GetCurrentUserTopTracks(
        string timeRange,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Artist>> GetCurrentUserTopArtists(
        string timeRange,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<User> GetUserProfile(
        string userId,
        string accessToken,
        CancellationToken cancellationToken = default);
}
