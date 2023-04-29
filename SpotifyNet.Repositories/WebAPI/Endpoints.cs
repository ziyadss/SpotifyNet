using System.Collections.Generic;

namespace SpotifyNet.Repositories.WebAPI;

internal static class Endpoints
{
    // Albums
    public static string GetAlbum(string albumId) => $"https://api.spotify.com/v1/albums/{albumId}";
    public static string GetSeveralAlbums(IEnumerable<string> albumIds) => $"https://api.spotify.com/v1/albums?ids={string.Join(',', albumIds)}";
    public static string GetAlbumTracks(string albumId) => $"https://api.spotify.com/v1/albums/{albumId}/tracks";
    public static string GetUserSavedAlbums() => $"https://api.spotify.com/v1/me/albums";
    public static string SaveAlbumsForUser() => $"https://api.spotify.com/v1/me/albums";
    public static string RemoveUserSavedAlbums() => $"https://api.spotify.com/v1/me/albums";
    public static string CheckUserSavedAlbums(IEnumerable<string> albumIds) => $"https://api.spotify.com/v1/me/albums/contains?ids={string.Join(',', albumIds)}";
    public static string GetNewReleases() => $"https://api.spotify.com/v1/browse/new-releases";

    // Artists
    public static string GetArtist(string artistId) => $"https://api.spotify.com/v1/artists/{artistId}";
    public static string GetSeveralArtists(IEnumerable<string> artistIds) => $"https://api.spotify.com/v1/artists?ids={string.Join(',', artistIds)}";

    // Audiobooks

    // Categories

    // Chapters

    // Episodes

    // Genres

    // Markets

    // Player
    public static string GetRecentlyPlayedTracks() => $"https://api.spotify.com/v1/me/player/recently-played";

    // Playlists
    public static string GetCurrentUserPlaylists() => $"https://api.spotify.com/v1/me/playlists";
    public static string GetPlaylistItems(string playlistId) => $"https://api.spotify.com/v1/playlists/{playlistId}/tracks";
    public static string GetUserPlaylists(string userId) => $"https://api.spotify.com/v1/users/{userId}/playlists";

    // Search

    // Shows

    // Tracks
    public static string GetTrack(string trackId) => $"https://api.spotify.com/v1/tracks/{trackId}";
    public static string GetSeveralTracks(IEnumerable<string> trackIds) => $"https://api.spotify.com/v1/tracks?ids={string.Join(',', trackIds)}";
    public static string GetUserSavedTracks() => $"https://api.spotify.com/v1/me/tracks";
    public static string GetTracksAudioFeatures(IEnumerable<string> trackIds) => $"https://api.spotify.com/v1/audio-features?ids={string.Join(',', trackIds)}";
    public static string GetTrackAudioFeatures(string trackId) => $"https://api.spotify.com/v1/audio-features/{trackId}";
    public static string GetTrackAudioAnalysis(string trackId) => $"https://api.spotify.com/v1/audio-analysis/{trackId}";

    // Users
    public static string GetCurrentUserTopItems(string type, string timeRange) => $"https://api.spotify.com/v1/me/top/{type}?time_range={timeRange}";
}
