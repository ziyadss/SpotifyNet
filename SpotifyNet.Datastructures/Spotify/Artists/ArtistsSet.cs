using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Artists;

public record ArtistsSet
{
    [JsonPropertyName("artists")]
    public required Artist[] Artists { get; init; }
}
