using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Categories;

public record Category
{
    [JsonPropertyName("href")]
    public required string Href { get; init; }

    [JsonPropertyName("icons")]
    public required Image[] Icons { get; init; }

    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
