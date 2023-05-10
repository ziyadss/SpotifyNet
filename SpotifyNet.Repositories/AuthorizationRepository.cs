using SpotifyNet.Clients.Interfaces;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Internal;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        IEnumerable<string> scopes,
        CancellationToken cancellationToken)
    {
        var scopesCollection = scopes as ICollection<string> ?? scopes.ToList();

        var authorization = await _authorizationClient.GetUserAuthorizeUri(scopesCollection, cancellationToken);

        var authorizationUri = await WriteAndReturnAuthorizationUri(authorization, scopesCollection, cancellationToken);

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

    public Task<bool> AccessTokenExists(
        CancellationToken cancellationToken)
    {
        var exists = File.Exists(AccessTokenFilePath);

        return Task.FromResult(exists);
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
        ICollection<string> scopes,
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
        ICollection<string> scopes,
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
        string path,
        CancellationToken cancellationToken)
    {
        using var fs = File.OpenRead(path);
        var item = await JsonSerializer.DeserializeAsync<T>(fs, cancellationToken: cancellationToken);
        return item!;
    }

    private static async Task Write<T>(
        string path,
        T item,
        CancellationToken cancellationToken)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            var directory = Path.GetDirectoryName(path)!;
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        using var fs = File.OpenWrite(path);
        await JsonSerializer.SerializeAsync(fs, item, cancellationToken: cancellationToken);
    }
}
