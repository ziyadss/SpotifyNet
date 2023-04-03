using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks.Analysis;

public class AudioAnalysis
{
    [JsonPropertyName("meta")]
    public AnalysisMeta? Meta { get; init; }

    [JsonPropertyName("track")]
    public AnalysisTrack? Track { get; init; }

    [JsonPropertyName("bars")]
    public AnalysisBar[]? Bars { get; init; }

    [JsonPropertyName("beats")]
    public AnalysisBeat[]? Beats { get; init; }

    [JsonPropertyName("sections")]
    public AnalysisSection[]? Sections { get; init; }

    [JsonPropertyName("segments")]
    public AnalysisSegment[]? Segments { get; init; }

    [JsonPropertyName("tatums")]
    public AnalysisTatum[]? Tatums { get; init; }
}
