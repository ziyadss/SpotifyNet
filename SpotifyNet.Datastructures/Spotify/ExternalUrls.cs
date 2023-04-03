using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public class ExternalUrls
{
    [JsonPropertyName("spotify")]
    public string? Spotify { get; init; }
}
