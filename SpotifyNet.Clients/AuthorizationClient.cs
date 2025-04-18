﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Core.Exceptions;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Authorization;

namespace SpotifyNet.Clients;

public class AuthorizationClient : IAuthorizationClient
{
    private const string AuthorizeEndpoint = "https://accounts.spotify.com/authorize";
    private const string TokenEndpoint = "https://accounts.spotify.com/api/token";

    private readonly string _appClientId;
    private readonly string _appRedirectUri;

    private readonly HttpClient _httpClient;

    public AuthorizationClient(string appClientId, string appRedirectUri, HttpClient httpClient)
    {
        _appClientId = appClientId;
        _appRedirectUri = appRedirectUri;

        _httpClient = httpClient;
    }

    public async Task<UserAuthorization> GetUserAuthorizeUri(
        IEnumerable<string> scopes,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scopes, nameof(scopes));

        var scopesCollection = scopes.ToCollection();
        var invalidScopes = scopesCollection.Except(AuthorizationScope.ValidScopes).ToList();

        if (invalidScopes.Count != 0)
        {
            throw new AuthorizationException($"Invalid authorization scopes `{string.Join(", ", invalidScopes)}`");
        }

        var (verifier, challenge) = PKCE.GetCodeVerifierAndChallenge(verifierLength: 64);
        var state = Guid.NewGuid().ToString();

        var parameters = new NameValueCollection
        {
            ["client_id"] = _appClientId,
            ["response_type"] = "code",
            ["redirect_uri"] = _appRedirectUri,
            ["state"] = state,
            ["scope"] = string.Join(',', scopesCollection),
            ["show_dialog"] = "false",
            ["code_challenge_method"] = "S256",
            ["code_challenge"] = challenge,
        };

        var builder = new UriBuilder(AuthorizeEndpoint);
        var queryParameters = HttpUtility.ParseQueryString(builder.Query);
        queryParameters.Add(parameters);
        builder.Query = queryParameters.ToString();

        using var response = await _httpClient.GetAsync(builder.Uri, cancellationToken);
        await Ensure.RequestSuccess(response, cancellationToken);

        return new()
        {
            AuthorizationUri = response.RequestMessage!.RequestUri!.AbsoluteUri,
            CodeVerifier = verifier,
            State = state,
        };
    }

    public Task<AccessToken> GetUserAccessToken(string code, string codeVerifier, CancellationToken cancellationToken)
    {
        var payload = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = _appRedirectUri,
            ["client_id"] = _appClientId,
            ["code_verifier"] = codeVerifier,
        };

        return PostForAccessToken(payload, cancellationToken);
    }

    public Task<AccessToken> RefreshUserAccessToken(string refreshToken, CancellationToken cancellationToken)
    {
        var payload = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = refreshToken,
            ["client_id"] = _appClientId,
        };

        return PostForAccessToken(payload, cancellationToken);
    }

    private async Task<AccessToken> PostForAccessToken(
        IReadOnlyDictionary<string, string> payload,
        CancellationToken cancellationToken)
    {
        using var content = new FormUrlEncodedContent(payload);

        using var response = await _httpClient.PostAsync(TokenEndpoint, content, cancellationToken);
        await Ensure.RequestSuccess(response, cancellationToken);

        var token = await response.Content.ReadFromJsonAsync<AccessToken>(cancellationToken: cancellationToken);

        return token!;
    }
}
