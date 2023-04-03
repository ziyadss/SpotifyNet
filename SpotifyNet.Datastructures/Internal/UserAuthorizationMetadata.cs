namespace SpotifyNet.Datastructures.Internal;

public class UserAuthorizationMetadata
{
    public required string AuthorizationUrl { get; init; }

    public required string CodeVerifier { get; init; }

    public required string State { get; init; }
}
