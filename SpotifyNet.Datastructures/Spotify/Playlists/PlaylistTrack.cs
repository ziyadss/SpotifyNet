﻿using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Users;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

public class PlaylistTrack
{
    [JsonPropertyName("collaborative")]
    public bool? Collaborative { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("images")]
    public Image[]? Images { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("owner")]
    public User? Owner { get; init; }

    [JsonPropertyName("public")]
    public bool? Public { get; init; }

    [JsonPropertyName("snapshot_id")]
    public string? SnapshotId { get; init; }

    [JsonPropertyName("tracks")]
    public PlaylistTracks? Tracks { get; init; }

    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}

public class PlaylistTracks
{
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("total")]
    public int? Total { get; init; }
}
