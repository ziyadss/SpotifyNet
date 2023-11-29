using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Internal;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Repositories.Abstractions;

namespace SpotifyNet.Repositories;

public class AuthorizationRepository : IAuthorizationRepository
{
    private const string AuthorizationMetadataFilePath = "AuthorizationMetadata.json";
    private const string AccessTokenFilePath = "AccessToken.json";

    private readonly IAuthorizationClient _authorizationClient;

    public AuthorizationRepository(IAuthorizationClient authorizationClient)
    {
        _authorizationClient = authorizationClient;
    }

    public async Task<string> GetUserAuthorizeUri(IEnumerable<string> scopes, CancellationToken cancellationToken)
    {
        var scopesCollection = scopes as ICollection<string> ?? scopes.ToList();

        var authorization = await _authorizationClient.GetUserAuthorizeUri(scopesCollection, cancellationToken).ConfigureAwait(false);

        var authorizationUri = await WriteAndReturnAuthorizationUri(authorization, scopesCollection, cancellationToken).ConfigureAwait(false);

        return authorizationUri;
    }

    public async Task<AccessTokenMetadata> GetNewAccessToken(
        string code,
        string state,
        CancellationToken cancellationToken)
    {
        var authorizationMetadata =
            await Read<UserAuthorizationMetadata>(AuthorizationMetadataFilePath, cancellationToken).ConfigureAwait(false);

        Ensure.Equal(state, authorizationMetadata.State);

        var token = await _authorizationClient.GetUserAccessToken(code, authorizationMetadata.CodeVerifier,
                                                                  cancellationToken).ConfigureAwait(false);

        var tokenMetadata =
            await WriteAndReturnToken(token, authorizationMetadata.AuthorizationScopes, cancellationToken).ConfigureAwait(false);

        return tokenMetadata;
    }

    public async Task<AccessTokenMetadata> GetAccessToken(CancellationToken cancellationToken)
    {
        var token = await Read<AccessTokenMetadata>(AccessTokenFilePath, cancellationToken).ConfigureAwait(false);

        if (token.IsExpiring)
        {
            token = await RefreshUserAccessToken(token, cancellationToken).ConfigureAwait(false);
        }

        return token;
    }

    public Task<bool> AccessTokenExists(CancellationToken cancellationToken)
    {
        var exists = File.Exists(AccessTokenFilePath);

        return Task.FromResult(exists);
    }

    private async Task<AccessTokenMetadata> RefreshUserAccessToken(
        AccessTokenMetadata existingTokenMetadata,
        CancellationToken cancellationToken)
    {
        var token = await _authorizationClient.RefreshUserAccessToken(existingTokenMetadata.RefreshToken,
                                                                      cancellationToken).ConfigureAwait(false);

        var tokenMetadata =
            await WriteAndReturnToken(token, existingTokenMetadata.AuthorizationScopes, cancellationToken).ConfigureAwait(false);

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
            CodeVerifier = authorization.CodeVerifier,
            State = authorization.State,
        };

        await Write(AuthorizationMetadataFilePath, authorizationMetadata, cancellationToken).ConfigureAwait(false);

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

        await Write(AccessTokenFilePath, tokenMetadata, cancellationToken).ConfigureAwait(false);

        return tokenMetadata;
    }

    private static async Task<T> Read<T>(string path, CancellationToken cancellationToken)
    {
        var fs = File.OpenRead(path);
        await using var _ = fs.ConfigureAwait(false);
        var item = await JsonSerializer.DeserializeAsync<T>(fs, cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }

    private static async Task Write<T>(string path, T item, CancellationToken cancellationToken)
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

        var fs = File.OpenWrite(path);
        await using var _ = fs.ConfigureAwait(false);
        await JsonSerializer.SerializeAsync(fs, item, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
