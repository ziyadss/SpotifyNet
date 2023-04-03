using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

public class SimplifiedPlaylist
{
    [JsonPropertyName("collaborative")]
    public required bool Collaborative { get; init; }

    [JsonPropertyName("description")]
    public required string? Description { get; init; }

    [JsonPropertyName("href")]
    public required string Href { get; init; }

    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("images")]
    public required Image[] Images { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("owner")]
    public required SimplifiedPlaylistUserObject Owner { get; init; }

    [JsonPropertyName("public")]
    public required bool? Public { get; init; }

    [JsonPropertyName("snapshot_id")]
    public required string SnapshotId { get; init; }

    [JsonPropertyName("tracks")]
    public required Followers Tracks { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

}

public class SimplifiedPlaylistUserObject
{
    [JsonPropertyName("href")]
    public required string Href { get; init; }

    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    [JsonPropertyName("display_name")]
    public required string? DisplayName { get; init; }
}
