﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Authorization;

namespace SpotifyNet.Clients.Abstractions;

public interface IAuthorizationClient
{
    Task<UserAuthorization> GetUserAuthorizeUri(
        IEnumerable<string> scopes,
        CancellationToken cancellationToken = default);

    Task<AccessToken> GetUserAccessToken(
        string code,
        string codeVerifier,
        CancellationToken cancellationToken = default);

    Task<AccessToken> RefreshUserAccessToken(string refreshToken, CancellationToken cancellationToken = default);
}
