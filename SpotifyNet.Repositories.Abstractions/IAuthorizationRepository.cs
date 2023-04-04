using SpotifyNet.Datastructures.Internal;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.Interfaces;

public interface IAuthorizationRepository
{
    Task<UserAuthorizationMetadata> ReadAuthorizationMetadata(CancellationToken cancellationToken = default);

    Task WriteAuthorizationMetadata(UserAuthorizationMetadata userAuthorizationMetadata, CancellationToken cancellationToken = default);

    Task<bool> AccessTokenExists(CancellationToken cancellationToken = default);

    Task<AccessTokenMetadata> ReadAccessToken(CancellationToken cancellationToken = default);

    Task WriteAccessToken(AccessTokenMetadata accessTokenMetadata, CancellationToken cancellationToken = default);
}
