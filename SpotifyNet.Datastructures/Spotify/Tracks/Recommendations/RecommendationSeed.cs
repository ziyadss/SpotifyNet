using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks.Recommendations;

public class RecommendationSeed
{
    [JsonPropertyName("afterFilteringSize")]
    public int? AfterFilteringSize { get; init; }

    [JsonPropertyName("afterRelinkingSize")]
    public int? AfterRelinkingSize { get; init; }

    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("initialPoolSize")]
    public int? InitialPoolSize { get; init; }

    [JsonPropertyName("type")]
    public RecommendationSeedType? Type { get; init; }
}
