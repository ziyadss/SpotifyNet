using System.Threading.Tasks;
using System.Threading;

namespace SpotifyNet.Auth.Interfaces;

public interface IAuthorizationService
{
    Task<string> GetUserAuthorizeUrl(string[] scopes, CancellationToken cancellationToken = default);

    Task<string> GetNewAccessToken(string code, string state, CancellationToken cancellationToken = default);

    Task<string> GetExistingAccessToken(CancellationToken cancellationToken = default);

    Task<string> RefreshUserAccessToken(CancellationToken cancellationToken = default);
}
