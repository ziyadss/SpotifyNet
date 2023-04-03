namespace SpotifyNet.WebAPI;

internal static class Endpoints
{
    // Albums

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
    public static string GetUserSavedTracks() => $"https://api.spotify.com/v1/me/tracks";
    public static string GetTracksAudioFeatures(string[] trackIds) => $"https://api.spotify.com/v1/audio-features?ids={string.Join(',', trackIds)}";

    // Users
}
