using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces;

public interface IAuthorizationService
{
    Task<string> GetUserAuthorizeUri(string[] scopes, CancellationToken cancellationToken = default);

    Task<string> GetNewAccessToken(string code, string state, CancellationToken cancellationToken = default);

    Task<string> GetExistingAccessToken(CancellationToken cancellationToken = default);

    Task<string> RefreshUserAccessToken(CancellationToken cancellationToken = default);
}
