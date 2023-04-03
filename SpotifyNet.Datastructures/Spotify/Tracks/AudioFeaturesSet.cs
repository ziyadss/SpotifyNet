using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks;

public class AudioFeaturesSet
{
    [JsonPropertyName("audio_features")]
    public required AudioFeatures[] AudioFeatures { get; init; }
}
