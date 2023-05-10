using System;
using System.Collections.Generic;

namespace SpotifyNet.Datastructures.Internal;

public class AccessTokenMetadata
{
    public required ICollection<string> AuthorizationScopes { get; init; }

    public required string Token { get; init; }

    public required DateTimeOffset ExpiresAtUtc { get; init; }

    public required string RefreshToken { get; init; }

    public bool IsExpiring => ExpiresAtUtc <= DateTimeOffset.UtcNow.AddMinutes(1);
}
