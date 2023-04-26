namespace SpotifyNet.Datastructures.Spotify.Authorization;

public record UserAuthorization
{
    public required string AuthorizationUri { get; init; }

    public required string CodeVerifier { get; init; }

    public required string State { get; init; }
}