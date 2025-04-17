using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Player;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Tracks.Analysis;
using SpotifyNet.Datastructures.Spotify.Users;

namespace SpotifyNet.Repositories.Abstractions;

public interface IWebAPIRepository
{
    // Albums
    Task<Album> GetAlbum(string albumId, string accessToken, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Album>> GetAlbums(
        IEnumerable<string> albumIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SimplifiedTrack>> GetAlbumTracks(
        string albumId,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SavedAlbum>> GetSavedAlbums(string accessToken, CancellationToken cancellationToken = default);

    Task SaveAlbums(IEnumerable<string> albumIds, string accessToken, CancellationToken cancellationToken = default);

    Task RemoveAlbums(IEnumerable<string> albumIds, string accessToken, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<bool>> AreAlbumsSaved(
        IEnumerable<string> albumIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SimplifiedAlbum>> GetNewReleases(
        string accessToken,
        CancellationToken cancellationToken = default);

    // Artists
    Task<Artist> GetArtist(string artistId, string accessToken, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Artist>> GetArtists(
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
    Task SetPlaybackVolume(int volume, string accessToken, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlayHistory>> GetRecentlyPlayedTracks(
        string accessToken,
        CancellationToken cancellationToken = default);

    // Playlists
    Task<Playlist> GetPlaylist(
        string playlistId,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlaylistTrack>> GetPlaylistItems(
        string playlistId,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SimplifiedPlaylist>> GetUserPlaylists(
        string userId,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task AddCustomPlaylistCoverImage(
        string playlistId,
        string base64Image,
        string accessToken,
        CancellationToken cancellationToken = default);

    // Search

    // Shows

    // Tracks
    Task<Track> GetTrack(string trackId, string accessToken, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Track>> GetTracks(
        IEnumerable<string> trackIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SavedTrack>> GetCurrentUserSavedTracks(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AudioFeatures>> GetTracksAudioFeatures(
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

    Task<IReadOnlyList<bool>> AreTracksSaved(
        IEnumerable<string> trackIds,
        string accessToken,
        CancellationToken cancellationToken = default);

    // Users
    Task<User> GetCurrentUserProfile(string accessToken, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Track>> GetCurrentUserTopTracks(
        string timeRange,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Artist>> GetCurrentUserTopArtists(
        string timeRange,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<User> GetUserProfile(string userId, string accessToken, CancellationToken cancellationToken = default);
}
