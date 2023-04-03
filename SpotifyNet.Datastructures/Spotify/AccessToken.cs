using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public class AccessToken
{
    [JsonPropertyName("access_token")]
    public required string Token { get; init; }

    [JsonPropertyName("token_type")]
    public required string TokenType { get; init; }

    [JsonPropertyName("scope")]
    public string? Scope { get; init; }

    [JsonPropertyName("expires_in")]
    public required int ExpiresIn { get; init; }

    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; init; }

}
