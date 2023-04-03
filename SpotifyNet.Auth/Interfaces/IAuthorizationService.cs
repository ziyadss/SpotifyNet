using System.Threading.Tasks;
using System.Threading;

namespace SpotifyNet.Auth.Interfaces;

public interface IAuthorizationService
{
    Task<string> GetUserAuthorizeUrl(string[] scopes, CancellationToken cancellationToken);

    Task<string> GetNewAccessToken(string code, string state, CancellationToken cancellationToken);

    Task<string> GetExistingAccessToken(CancellationToken cancellationToken);

    Task<string> RefreshUserAccessToken(CancellationToken cancellationToken);
}
