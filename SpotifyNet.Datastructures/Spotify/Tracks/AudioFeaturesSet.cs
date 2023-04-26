using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks;

public record AudioFeaturesSet
{
    [JsonPropertyName("audio_features")]
    public required AudioFeatures[] AudioFeatures { get; init; }
}
