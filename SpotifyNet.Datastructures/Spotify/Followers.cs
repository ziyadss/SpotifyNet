using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public record Followers
{
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("total")]
    public int? Total { get; init; }
}
