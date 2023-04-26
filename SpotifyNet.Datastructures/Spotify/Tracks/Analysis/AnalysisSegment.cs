using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks.Analysis;

public record AnalysisSegment
{
    [JsonPropertyName("start")]
    public double? Start { get; init; }

    [JsonPropertyName("duration")]
    public double? Duration { get; init; }

    [JsonPropertyName("confidence")]
    public double? Confidence { get; init; }

    [JsonPropertyName("loudness_start")]
    public double? LoudnessStart { get; init; }

    [JsonPropertyName("loudness_max")]
    public double? LoudnessMax { get; init; }

    [JsonPropertyName("loudness_max_time")]
    public double? LoudnessMaxTime { get; init; }

    [JsonPropertyName("loudness_end")]
    public double? LoudnessEnd { get; init; }

    [JsonPropertyName("pitches")]
    public double[]? Pitches { get; init; }

    [JsonPropertyName("timbre")]
    public double[]? Timbre { get; init; }
}
