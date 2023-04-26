using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public record PaginationWrapper<T>
{
    [JsonPropertyName("href")]
    public required string Href { get; init; }

    [JsonPropertyName("limit")]
    public required int Limit { get; init; }

    [JsonPropertyName("next")]
    public required string? Next { get; init; }

    [JsonPropertyName("offset")]
    public required int Offset { get; init; }

    [JsonPropertyName("previous")]
    public required string? Previous { get; init; }

    [JsonPropertyName("total")]
    public required int Total { get; init; }

    [JsonPropertyName("items")]
    public required T[] Items { get; init; }
}
