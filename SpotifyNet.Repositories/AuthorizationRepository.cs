using SpotifyNet.Clients.Interfaces;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Internal;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Repositories.Interfaces;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.Authorization;

public class AuthorizationRepository : IAuthorizationRepository
{
    private const string AuthorizationMetadataFilePath = "AuthorizationMetadata.json";
    private const string AccessTokenFilePath = "AccessToken.json";

    private readonly IAuthorizationClient _authorizationClient;

    public AuthorizationRepository(
        IAuthorizationClient authorizationClient)
    {
        _authorizationClient = authorizationClient;
    }

    public async Task<string> GetUserAuthorizeUri(
        string[] scopes,
        CancellationToken cancellationToken)
    {
        var authorization = await _authorizationClient.GetUserAuthorizeUri(scopes, cancellationToken);

        var authorizationUri = await WriteAndReturnAuthorizationUri(authorization, scopes, cancellationToken);

        return authorizationUri;
    }

    public async Task<AccessTokenMetadata> GetNewAccessToken(
        string code,
        string state,
        CancellationToken cancellationToken)
    {
        var authorizationMetadata = await Read<UserAuthorizationMetadata>(AuthorizationMetadataFilePath, cancellationToken);

        Ensure.Equal(state, authorizationMetadata.State);

        var token = await _authorizationClient.GetUserAccessToken(code, authorizationMetadata.CodeVerifier, cancellationToken);

        var tokenMetadata = await WriteAndReturnToken(token, authorizationMetadata.AuthorizationScopes, cancellationToken);

        return tokenMetadata;
    }

    public async Task<AccessTokenMetadata> GetAccessToken(
        CancellationToken cancellationToken)
    {
        var token = await Read<AccessTokenMetadata>(AccessTokenFilePath, cancellationToken);

        if (token.IsExpiring)
        {
            token = await RefreshUserAccessToken(token, cancellationToken);
        }

        return token;
    }

    private async Task<AccessTokenMetadata> RefreshUserAccessToken(
        AccessTokenMetadata existingTokenMetadata,
        CancellationToken cancellationToken)
    {
        var token = await _authorizationClient.RefreshUserAccessToken(existingTokenMetadata.RefreshToken, cancellationToken);

        var tokenMetadata = await WriteAndReturnToken(token, existingTokenMetadata.AuthorizationScopes, cancellationToken);

        return tokenMetadata;
    }

    private static async Task<string> WriteAndReturnAuthorizationUri(
        UserAuthorization authorization,
        string[] scopes,
        CancellationToken cancellationToken)
    {
        var authorizationMetadata = new UserAuthorizationMetadata
        {
            AuthorizationScopes = scopes,
            AuthorizationUri = authorization.AuthorizationUri,
            CodeVerifier = authorization.CodeVerifier,
            State = authorization.State,
        };

        await Write(AuthorizationMetadataFilePath, authorizationMetadata, cancellationToken);

        return authorization.AuthorizationUri;
    }

    private static async Task<AccessTokenMetadata> WriteAndReturnToken(
        AccessToken token,
        string[] scopes,
        CancellationToken cancellationToken)
    {
        var tokenMetadata = new AccessTokenMetadata
        {
            AuthorizationScopes = scopes,
            Token = token.Token,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn),
            RefreshToken = token.RefreshToken,
        };

        await Write(AccessTokenFilePath, tokenMetadata, cancellationToken);

        return tokenMetadata;
    }

    private static async Task<T> Read<T>(
        string filePath,
        CancellationToken cancellationToken)
    {
        using var fs = File.OpenRead(filePath);

        var item = await JsonSerializer.DeserializeAsync<T>(fs, cancellationToken: cancellationToken);

        return item!;
    }

    private static Task Write<T>(
        string filePath,
        T item,
        CancellationToken cancellationToken)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        using var fs = File.OpenWrite(filePath);

        return JsonSerializer.SerializeAsync(fs, item, cancellationToken: cancellationToken);
    }
}
