using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public record Restrictions
{
    [JsonPropertyName("reason")]
    public RestrictionReason Reason { get; init; }
}
