using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Common;

public interface ITokenAcquirer
{
    Task EnsureTokenExists(
        string[] scopes,
        bool forceGenerate = false,
        CancellationToken cancellationToken = default);
}
