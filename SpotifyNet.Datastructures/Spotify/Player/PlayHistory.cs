using System;
using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Datastructures.Spotify.Player;

public record PlayHistory
{
    [JsonPropertyName("track")]
    public required Track Track { get; init; }

    [JsonPropertyName("played_at")]
    public required DateTime PlayedAt { get; init; }

    [JsonPropertyName("context")]
    public required Context? Context { get; init; }
}
