using System;

namespace SpotifyNet.Datastructures.Internal;

public class AccessTokenMetadata
{
    public required string Token { get; init; }

    public required DateTimeOffset ExpiresAtUtc { get; init; }

    public required string RefreshToken { get; init; }

    public bool IsExpiring => ExpiresAtUtc <= DateTimeOffset.UtcNow.AddMinutes(1);
}
