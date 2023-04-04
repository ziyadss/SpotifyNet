using SpotifyNet.Clients.Interfaces;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Internal;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Auth;

public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationClient _authorizationClient;
    private readonly IAuthorizationRepository _authorizationRepository;

    public AuthorizationService(
        IAuthorizationClient authorizationClient,
        IAuthorizationRepository authorizationRepository)
    {
        _authorizationClient = authorizationClient;
        _authorizationRepository = authorizationRepository;
    }

    public async Task<string> GetUserAuthorizeUrl(string[] scopes, CancellationToken cancellationToken)
    {
        var authorization = await _authorizationClient.GetUserAuthorizeUrl(scopes, cancellationToken);

        var authorizationUrl = await WriteAndReturnAuthorizationUrl(authorization, scopes, cancellationToken);

        return authorizationUrl;
    }

    public async Task<string> GetNewAccessToken(string code, string state, CancellationToken cancellationToken)
    {
        var metadata = await _authorizationRepository.ReadAuthorizationMetadata(cancellationToken);

        Ensure.Equal(state, metadata.State);

        var token = await _authorizationClient.GetUserAccessToken(code, metadata.CodeVerifier, cancellationToken);

        var tokenString = await WriteAndReturnToken(token, cancellationToken);

        return tokenString;
    }

    public async Task<string> GetExistingAccessToken(CancellationToken cancellationToken)
    {
        var token = await _authorizationRepository.ReadAccessToken(cancellationToken);

        string tokenString;
        if (token.IsExpiring)
        {
            tokenString = await RefreshUserAccessToken(token, cancellationToken);
        }
        else
        {
            tokenString = token.Token;
        }

        return tokenString;
    }

    public async Task<string> RefreshUserAccessToken(CancellationToken cancellationToken)
    {
        var token = await _authorizationRepository.ReadAccessToken(cancellationToken);

        var tokenString = await RefreshUserAccessToken(token, cancellationToken);

        return tokenString;
    }

    private async Task<string> RefreshUserAccessToken(AccessTokenMetadata existingTokenMetadata, CancellationToken cancellationToken)
    {
        var token = await _authorizationClient.RefreshUserAccessToken(existingTokenMetadata.RefreshToken, cancellationToken);

        var tokenString = await WriteAndReturnToken(token, cancellationToken);

        return tokenString;
    }

    private async Task<string> WriteAndReturnAuthorizationUrl(UserAuthorization authorization, string[] scopes, CancellationToken cancellationToken)
    {
        var authorizationMetadata = new UserAuthorizationMetadata
        {
            AuthorizationScopes = scopes,
            AuthorizationUrl = authorization.AuthorizationUrl,
            CodeVerifier = authorization.CodeVerifier,
            State = authorization.State,
        };

        await _authorizationRepository.WriteAuthorizationMetadata(authorizationMetadata, cancellationToken);

        return authorization.AuthorizationUrl;
    }

    private async Task<string> WriteAndReturnToken(AccessToken token, CancellationToken cancellationToken)
    {
        var tokenMetadata = new AccessTokenMetadata
        {
            Token = token.Token,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn),
            RefreshToken = token.RefreshToken,
        };

        await _authorizationRepository.WriteAccessToken(tokenMetadata, cancellationToken);

        return token.Token;
    }
}
