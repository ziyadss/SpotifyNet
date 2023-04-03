using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Users;

public class ExplicitContentSettings
{
    [JsonPropertyName("filter_enabled")]
    public bool? FilterEnabled { get; init; }

    [JsonPropertyName("filter_locked")]
    public bool? FilterLocked { get; init; }
}
