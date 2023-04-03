using SpotifyNet.Datastructures.Spotify.Authorization;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Auth.Interfaces;

public interface IAuthorizationClient
{
    Task<UserAuthorization> GetUserAuthorizeUrl(string[] scopes, CancellationToken cancellationToken = default);

    Task<AccessToken> GetUserAccessToken(string code, string codeVerifier, CancellationToken cancellationToken = default);

    Task<AccessToken> RefreshUserAccessToken(string refreshToken, CancellationToken cancellationToken = default);
}
