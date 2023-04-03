using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks.Analysis;

public class AnalysisSection
{
    [JsonPropertyName("start")]
    public double? Start { get; init; }

    [JsonPropertyName("duration")]
    public double? Duration { get; init; }

    [JsonPropertyName("confidence")]
    public double? Confidence { get; init; }

    [JsonPropertyName("loudness")]
    public double? Loudness { get; init; }

    [JsonPropertyName("tempo")]
    public double? Tempo { get; init; }

    [JsonPropertyName("tempo_confidence")]
    public double? TempoConfidence { get; init; }

    [JsonPropertyName("key")]
    public int? Key { get; init; }

    [JsonPropertyName("key_confidence")]
    public double? KeyConfidence { get; init; }

    [JsonPropertyName("mode")]
    public int? Mode { get; init; }

    [JsonPropertyName("mode_confidence")]
    public double? ModeConfidence { get; init; }

    [JsonPropertyName("time_signature")]
    public int? TimeSignature { get; init; }

    [JsonPropertyName("time_signature_confidence")]
    public double? TimeSignatureConfidence { get; init; }
}
