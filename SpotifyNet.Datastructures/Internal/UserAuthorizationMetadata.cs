namespace SpotifyNet.Datastructures.Internal;

public class UserAuthorizationMetadata
{
    public required string[] AuthorizationScopes { get; init; }

    public required string AuthorizationUri { get; init; }

    public required string CodeVerifier { get; init; }

    public required string State { get; init; }
}
