using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public class Restrictions
{
    [JsonPropertyName("reason")]
    public RestrictionReason Reason { get; init; }
}
