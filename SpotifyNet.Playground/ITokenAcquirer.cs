using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Playground;

internal interface ITokenAcquirer
{
    Task<string> GetToken(string[] scopes, CancellationToken cancellationToken = default);

    Task<string> GetExistingToken(CancellationToken cancellationToken = default);
}
