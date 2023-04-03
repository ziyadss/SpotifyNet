using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks.Recommendations;

public class RecommendationsResult
{
    [JsonPropertyName("seeds")]
    public required RecommendationSeed[] Seeds { get; init; }

    [JsonPropertyName("tracks")]
    public required Track[] Tracks { get; init; }
}
