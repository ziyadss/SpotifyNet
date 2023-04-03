using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SpotifyNet.Auth.Interfaces;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Authorization;

namespace SpotifyNet.Auth;

internal class AuthorizationClient : IAuthorizationClient
{
    private readonly string _appClientId;
    private readonly string _appRedirectUri;

    private const string AuthorizeEndpoint = "https://accounts.spotify.com/authorize";
    private const string TokenEndpoint = "https://accounts.spotify.com/api/token";

    private readonly HttpClient _httpClient;

    public AuthorizationClient(string appClientId, string appRedirectUri)
    {
        _appClientId = appClientId;
        _appRedirectUri = appRedirectUri;
        _httpClient = new();
    }

    public async Task<UserAuthorization> GetUserAuthorizeUrl(string[] scopes, CancellationToken cancellationToken)
    {
        foreach (var scope in scopes)
        {
            if (!AuthorizationScope.ValidScopes.Contains(scope))
            {
                throw new Exception($"Invalid scope: `{scope}`");
            }
        }

        var (verifier, challenge) = PKCE.GetCodeVerifierAndChallenge(verifierLength: 64);
        var state = Guid.NewGuid().ToString();

        var parameters = new NameValueCollection
        {
            ["client_id"] = _appClientId,
            ["response_type"] = "code",
            ["redirect_uri"] = _appRedirectUri,
            ["state"] = state,
            ["scope"] = string.Join(',', scopes),
            ["show_dialog"] = "false",
            ["code_challenge_method"] = "S256",
            ["code_challenge"] = challenge,
        };

        var builder = new UriBuilder(AuthorizeEndpoint);
        var queryParameters = HttpUtility.ParseQueryString(builder.Query);
        queryParameters.Add(parameters);
        builder.Query = queryParameters.ToString();

        using var response = await _httpClient.GetAsync(builder.Uri, cancellationToken);
        await Ensure.RequestSuccess(response, HttpStatusCode.OK);

        return new UserAuthorization
        {
            AuthorizationUrl = response.RequestMessage!.RequestUri!.AbsoluteUri,
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

    private async Task<AccessToken> PostForAccessToken(IReadOnlyDictionary<string, string> payload, CancellationToken cancellationToken)
    {
        using var content = new FormUrlEncodedContent(payload);

        using var response = await _httpClient.PostAsync(TokenEndpoint, content, cancellationToken);
        await Ensure.RequestSuccess(response, HttpStatusCode.OK);

        var token = await response.Content.ReadFromJsonAsync<AccessToken>(cancellationToken: cancellationToken);

        return token!;
    }
}
