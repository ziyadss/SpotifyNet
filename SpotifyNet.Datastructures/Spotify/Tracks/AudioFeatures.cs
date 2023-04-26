using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks;

public record AudioFeatures
{
    [JsonPropertyName("acousticness")]
    public double? Acousticness { get; init; }

    [JsonPropertyName("analysis_url")]
    public string? AnalysisUrl { get; init; }

    [JsonPropertyName("danceability")]
    public double? Danceability { get; init; }

    [JsonPropertyName("duration_ms")]
    public int? DurationMs { get; init; }

    [JsonPropertyName("energy")]
    public double? Energy { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("instrumentalness")]
    public double? Instrumentalness { get; init; }

    [JsonPropertyName("key")]
    public int? Key { get; init; }

    [JsonPropertyName("liveness")]
    public double? Liveness { get; init; }

    [JsonPropertyName("loudness")]
    public double? Loudness { get; init; }

    [JsonPropertyName("mode")]
    public int? Mode { get; init; }

    [JsonPropertyName("speechiness")]
    public double? Speechiness { get; init; }

    [JsonPropertyName("tempo")]
    public double? Tempo { get; init; }

    [JsonPropertyName("time_signature")]
    public int? TimeSignature { get; init; }

    [JsonPropertyName("track_href")]
    public string? TrackHref { get; init; }

    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    [JsonPropertyName("valence")]
    public double? Valence { get; init; }
}
