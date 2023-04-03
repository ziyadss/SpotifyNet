using System.Threading.Tasks;
using System.Threading;
using SpotifyNet.Datastructures.Spotify;

namespace SpotifyNet.Auth.Interfaces;

internal interface IAuthorizationClient
{
    Task<UserAuthorization> GetUserAuthorizeUrl(string[] scopes, CancellationToken cancellationToken);

    Task<AccessToken> GetUserAccessToken(string code, string codeVerifier, CancellationToken cancellationToken);

    Task<AccessToken> RefreshUserAccessToken(string refreshToken, CancellationToken cancellationToken);
}
