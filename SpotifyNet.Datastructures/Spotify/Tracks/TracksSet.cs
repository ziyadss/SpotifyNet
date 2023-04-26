using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks;

public record TracksSet
{
    [JsonPropertyName("tracks")]
    public required Track[] Tracks { get; init; }
}
