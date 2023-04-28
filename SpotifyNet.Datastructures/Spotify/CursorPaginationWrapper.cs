using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public record CursorPaginationWrapper<T>
{
    [JsonPropertyName("href")]
    public required string Href { get; init; }

    [JsonPropertyName("limit")]
    public required int Limit { get; init; }

    [JsonPropertyName("next")]
    public required string? Next { get; init; }

    [JsonPropertyName("cursors")]
    public required Cursors Cursors { get; init; }

    [JsonPropertyName("total")]
    public int? Total { get; init; }

    [JsonPropertyName("items")]
    public required T[] Items { get; init; }
}

public record Cursors
{
    [JsonPropertyName("after")]
    public required string? After { get; init; }

    [JsonPropertyName("before")]
    public required string? Before { get; init; }
}
