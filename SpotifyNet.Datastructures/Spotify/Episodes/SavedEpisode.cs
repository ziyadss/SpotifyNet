using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Episodes;

public record SavedEpisode
{
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    [JsonPropertyName("episode")]
    public Episode? Episode { get; init; }
}
