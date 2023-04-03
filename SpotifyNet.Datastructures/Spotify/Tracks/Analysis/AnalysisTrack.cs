using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks.Analysis;

public class AnalysisTrack
{
    [JsonPropertyName("num_samples")]
    public int? NumSamples { get; init; }

    [JsonPropertyName("duration")]
    public double? Duration { get; init; }

    [JsonPropertyName("sample_md5")]
    public string? SampleMd5 { get; init; }

    [JsonPropertyName("offset_seconds")]
    public int? OffsetSeconds { get; init; }

    [JsonPropertyName("window_seconds")]
    public int? WindowSeconds { get; init; }

    [JsonPropertyName("analysis_sample_rate")]
    public int? AnalysisSampleRate { get; init; }

    [JsonPropertyName("analysis_channels")]
    public int? AnalysisChannels { get; init; }

    [JsonPropertyName("end_of_fade_in")]
    public double? EndOfFadeIn { get; init; }

    [JsonPropertyName("start_of_fade_out")]
    public double? StartOfFadeOut { get; init; }

    [JsonPropertyName("loudness")]
    public double? Loudness { get; init; }

    [JsonPropertyName("tempo")]
    public double? Tempo { get; init; }

    [JsonPropertyName("tempo_confidence")]
    public double? TempoConfidence { get; init; }

    [JsonPropertyName("time_signature")]
    public int? TimeSignature { get; init; }

    [JsonPropertyName("time_signature_confidence")]
    public double? TimeSignatureConfidence { get; init; }

    [JsonPropertyName("key")]
    public int? Key { get; init; }

    [JsonPropertyName("key_confidence")]
    public double? KeyConfidence { get; init; }

    [JsonPropertyName("mode")]
    public int? Mode { get; init; }

    [JsonPropertyName("mode_confidence")]
    public double? ModeConfidence { get; init; }

    [JsonPropertyName("codestring")]
    public string? Codestring { get; init; }

    [JsonPropertyName("code_version")]
    public double? CodeVersion { get; init; }

    [JsonPropertyName("echoprintstring")]
    public string? Echoprintstring { get; init; }

    [JsonPropertyName("echoprint_version")]
    public double? EchoprintVersion { get; init; }

    [JsonPropertyName("synchstring")]
    public string? Synchstring { get; init; }

    [JsonPropertyName("synch_version")]
    public double? SynchVersion { get; init; }

    [JsonPropertyName("rhythmstring")]
    public string? Rhythmstring { get; init; }

    [JsonPropertyName("rhythm_version")]
    public double? RhythmVersion { get; init; }
}
