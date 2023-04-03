namespace SpotifyNet.Datastructures.Spotify.Authorization;

public class UserAuthorization
{
    public required string AuthorizationUrl { get; init; }

    public required string CodeVerifier { get; init; }

    public required string State { get; init; }
}