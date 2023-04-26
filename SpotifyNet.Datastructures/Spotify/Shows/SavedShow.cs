using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Shows;

public record SavedShow
{
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    [JsonPropertyName("show")]
    public Show? Show { get; init; }
}
