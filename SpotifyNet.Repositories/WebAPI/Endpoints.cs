using System.Collections.Generic;

namespace SpotifyNet.Repositories.WebAPI;

internal static class Endpoints
{
    private const string BaseUrl = $"https://api.spotify.com/v1";

    // Albums
    public static string GetAlbum(string albumId) => $"{BaseUrl}/albums/{albumId}";

    public static string GetSeveralAlbums(IEnumerable<string> albumIds) => $"{BaseUrl}/albums?ids={string.Join(',', albumIds)}";

    public static string GetAlbumTracks(string albumId) => $"{BaseUrl}/albums/{albumId}/tracks";
    public static string GetUserSavedAlbums() => $"{BaseUrl}/me/albums";
    public static string SaveAlbumsForUser() => $"{BaseUrl}/me/albums";
    public static string RemoveUserSavedAlbums() => $"{BaseUrl}/me/albums";

    public static string CheckUserSavedAlbums(IEnumerable<string> albumIds) => $"{BaseUrl}/me/albums/contains?ids={string.Join(',', albumIds)}";

    public static string GetNewReleases() => $"{BaseUrl}/browse/new-releases";

    // Artists
    public static string GetArtist(string artistId) => $"{BaseUrl}/artists/{artistId}";

    public static string GetSeveralArtists(IEnumerable<string> artistIds) => $"{BaseUrl}/artists?ids={string.Join(',', artistIds)}";

    // Audiobooks

    // Categories

    // Chapters

    // Episodes

    // Genres

    // Markets

    // Player
    public static string SetPlaybackVolume(int volume) => $"{BaseUrl}/me/player/volume?volume_percent={volume}";

    public static string SetPlaybackVolume(int volume, string deviceId) => $"{BaseUrl}/me/player/volume?volume_percent={volume}&device_id={deviceId}";

    public static string GetRecentlyPlayedTracks() => $"{BaseUrl}/me/player/recently-played";

    // Playlists
    public static string GetCurrentUserPlaylists() => $"{BaseUrl}/me/playlists";

    public static string GetPlaylistItems(string playlistId) => $"{BaseUrl}/playlists/{playlistId}/tracks";

    public static string GetUserPlaylists(string userId) => $"{BaseUrl}/users/{userId}/playlists";

    // Search

    // Shows

    // Tracks
    public static string GetTrack(string trackId) => $"{BaseUrl}/tracks/{trackId}";

    public static string GetSeveralTracks(IEnumerable<string> trackIds) => $"{BaseUrl}/tracks?ids={string.Join(',', trackIds)}";

    public static string GetUserSavedTracks() => $"{BaseUrl}/me/tracks";

    public static string CheckUserSavedTracks(IEnumerable<string> trackIds) => $"{BaseUrl}/me/tracks/contains?ids={string.Join(',', trackIds)}";

    public static string GetTracksAudioFeatures(IEnumerable<string> trackIds) => $"{BaseUrl}/audio-features?ids={string.Join(',', trackIds)}";

    public static string GetTrackAudioFeatures(string trackId) => $"{BaseUrl}/audio-features/{trackId}";

    public static string GetTrackAudioAnalysis(string trackId) => $"{BaseUrl}/audio-analysis/{trackId}";

    // Users
    public static string GetCurrentUserProfile() => $"{BaseUrl}/me";

    public static string GetCurrentUserTopItems(string type, string timeRange) => $"{BaseUrl}/me/top/{type}?time_range={timeRange}";

    public static string GetUserProfile(string userId) => $"{BaseUrl}/users/{userId}";
}
