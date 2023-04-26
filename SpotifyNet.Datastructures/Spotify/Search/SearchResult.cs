using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Audiobooks;
using SpotifyNet.Datastructures.Spotify.Episodes;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Shows;
using SpotifyNet.Datastructures.Spotify.Tracks;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Search;

public record SearchResult
{
    [JsonPropertyName("tracks")]
    public PaginationWrapper<Track>? Tracks { get; init; }

    [JsonPropertyName("artists")]
    public PaginationWrapper<Artist>? Artists { get; init; }

    [JsonPropertyName("albums")]
    public PaginationWrapper<SimplifiedAlbum>? Albums { get; init; }

    [JsonPropertyName("playlists")]
    public PaginationWrapper<SimplifiedPlaylist>? Playlists { get; init; }

    [JsonPropertyName("shows")]
    public PaginationWrapper<SimplifiedShow>? Shows { get; init; }

    [JsonPropertyName("episodes")]
    public PaginationWrapper<SimplifiedEpisode>? Episodes { get; init; }

    [JsonPropertyName("audiobooks")]
    public PaginationWrapper<SimplifiedAudiobook>? Audiobooks { get; init; }
}
