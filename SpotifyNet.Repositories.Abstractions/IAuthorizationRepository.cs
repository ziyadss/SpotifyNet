using SpotifyNet.Datastructures.Internal;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.Interfaces;

public interface IAuthorizationRepository
{
    Task<string> GetUserAuthorizeUri(
        string[] scopes,
        CancellationToken cancellationToken = default);

    Task<AccessTokenMetadata> GetNewAccessToken(
        string code,
        string state,
        CancellationToken cancellationToken = default);

    Task<AccessTokenMetadata> GetAccessToken(
        CancellationToken cancellationToken = default);
}
