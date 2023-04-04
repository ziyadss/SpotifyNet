using SpotifyNet.Datastructures.Spotify.Authorization;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Clients.Interfaces;

public interface IAuthorizationClient
{
    Task<UserAuthorization> GetUserAuthorizeUri(string[] scopes, CancellationToken cancellationToken = default);

    Task<AccessToken> GetUserAccessToken(string code, string codeVerifier, CancellationToken cancellationToken = default);

    Task<AccessToken> RefreshUserAccessToken(string refreshToken, CancellationToken cancellationToken = default);
}
