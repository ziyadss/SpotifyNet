using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks.Analysis;

public class AnalysisTatum
{
    [JsonPropertyName("start")]
    public double? Start { get; init; }

    [JsonPropertyName("duration")]
    public double? Duration { get; init; }

    [JsonPropertyName("confidence")]
    public double? Confidence { get; init; }
}
