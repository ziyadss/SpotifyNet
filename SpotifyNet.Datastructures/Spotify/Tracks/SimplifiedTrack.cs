﻿using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Artists;

namespace SpotifyNet.Datastructures.Spotify.Tracks;

public record SimplifiedTrack
{
    [JsonPropertyName("artists")]
    public SimplifiedArtist[]? Artists { get; init; }

    [JsonPropertyName("available_markets")]
    public string[]? AvailableMarkets { get; init; }

    [JsonPropertyName("disc_number")]
    public int? DiscNumber { get; init; }

    [JsonPropertyName("duration_ms")]
    public int? DurationMs { get; init; }

    [JsonPropertyName("explicit")]
    public bool? Explicit { get; init; }

    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("is_playable")]
    public bool? IsPlayable { get; init; }

    [JsonPropertyName("linked_from")]
    public LinkedFrom? LinkedFrom { get; init; }

    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("preview_url")]
    public string? PreviewUrl { get; init; }

    [JsonPropertyName("track_number")]
    public int? TrackNumber { get; init; }

    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    [JsonPropertyName("is_local")]
    public bool? IsLocal { get; init; }
}
