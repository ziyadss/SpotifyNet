using System.Text.Json.Serialization;
using System;

namespace SpotifyNet.Datastructures.Spotify.Episodes;

public class SavedEpisode
{
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    [JsonPropertyName("episode")]
    public Episode? Episode { get; init; }
}
