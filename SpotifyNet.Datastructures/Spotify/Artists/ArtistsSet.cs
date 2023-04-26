using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Artists;

public class ArtistsSet
{
    [JsonPropertyName("artists")]
    public required Artist[] Artists { get; init; }
}
