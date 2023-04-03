using SpotifyNet.Datastructures.Internal;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Auth.Interfaces;

internal interface IAuthorizationRepository
{
    Task<UserAuthorizationMetadata> ReadAuthorizationMetadata(CancellationToken cancellationToken);

    Task WriteAuthorizationMetadata(UserAuthorizationMetadata userAuthorizationMetadata, CancellationToken cancellationToken);

    Task<bool> AccessTokenExists(CancellationToken cancellationToken);

    Task<AccessTokenMetadata> ReadAccessToken(CancellationToken cancellationToken);

    Task WriteAccessToken(AccessTokenMetadata accessTokenMetadata, CancellationToken cancellationToken);
}
