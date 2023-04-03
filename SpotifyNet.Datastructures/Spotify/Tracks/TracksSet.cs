using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks;

public class TracksSet
{
    [JsonPropertyName("tracks")]
    public required Track[] Tracks { get; init; }
}
