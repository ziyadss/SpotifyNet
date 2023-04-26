using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Albums;

public record Album
{
    [JsonPropertyName("album_type")]
    public required AlbumType AlbumType { get; init; }

    [JsonPropertyName("total_tracks")]
    public required int TotalTracks { get; init; }

    [JsonPropertyName("available_markets")]
    public required string[] AvailableMarkets { get; init; }

    [JsonPropertyName("external_urls")]
    public required ExternalUrls ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public required string Href { get; init; }

    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("images")]
    public required Image[] Images { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("release_date")]
    public required string ReleaseDate { get; init; }

    [JsonPropertyName("release_date_precision")]
    public required ReleaseDatePrecision ReleaseDatePrecision { get; init; }

    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; init; }

    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    [JsonPropertyName("copyrights")]
    public Copyright[]? Copyrights { get; init; }

    [JsonPropertyName("external_ids")]
    public ExternalIds? ExternalIds { get; init; }

    [JsonPropertyName("genres")]
    public string[]? Genres { get; init; }

    [JsonPropertyName("label")]
    public string? Label { get; init; }

    [JsonPropertyName("popularity")]
    public int? Popularity { get; init; }

    [JsonPropertyName("artists")]
    public required Artist[] Artists { get; init; }

    [JsonPropertyName("tracks")]
    public required PaginationWrapper<SimplifiedTrack> Tracks { get; init; }
}
