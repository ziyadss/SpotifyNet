using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Playground;

internal interface ITokenAcquirer
{
    Task GenerateToken(
        string[] scopes,
        CancellationToken cancellationToken = default);
}
