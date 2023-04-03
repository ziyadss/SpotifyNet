namespace SpotifyNet.WebAPI;

internal static class Endpoints
{
    // Albums
    public static string GetAlbum(string albumId) => $"https://api.spotify.com/v1/albums/{albumId}";
    public static string GetSeveralAlbums(string[] albumIds) => $"https://api.spotify.com/v1/albums?ids={string.Join(',', albumIds)}";
    public static string GetAlbumTracks(string albumId) => $"https://api.spotify.com/v1/albums/{albumId}/tracks";
    public static string GetUserSavedAlbums() => $"https://api.spotify.com/v1/me/albums";
    public static string SaveAlbumsForUser() => $"https://api.spotify.com/v1/me/albums";
    public static string RemoveUserSavedAlbums() => $"https://api.spotify.com/v1/me/albums";
    public static string CheckUserSavedAlbums(string[] albumIds) => $"https://api.spotify.com/v1/me/albums/contains?ids={string.Join(',', albumIds)}";

    // Artists

    // Audiobooks

    // Categories

    // Chapters

    // Episodes

    // Genres

    // Markets

    // Player

    // Playlists
    public static string GetCurrentUserPlaylists() => $"https://api.spotify.com/v1/me/playlists";
    public static string GetPlaylistItems(string playlistId) => $"https://api.spotify.com/v1/playlists/{playlistId}/tracks";

    // Search

    // Shows

    // Tracks
    public static string GetSeveralTracks(string[] trackIds) => $"https://api.spotify.com/v1/tracks?ids={string.Join(',', trackIds)}";
    public static string GetUserSavedTracks() => $"https://api.spotify.com/v1/me/tracks";
    public static string GetTracksAudioFeatures(string[] trackIds) => $"https://api.spotify.com/v1/audio-features?ids={string.Join(',', trackIds)}";
    public static string GetTrackAudioAnalysis(string trackId) => $"https://api.spotify.com/v1/audio-analysis/{trackId}";

    // Users
}
