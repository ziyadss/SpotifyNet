using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Common;

public interface ITokenAcquirer
{
    Task GenerateToken(
        string[] scopes,
        CancellationToken cancellationToken = default);
}
