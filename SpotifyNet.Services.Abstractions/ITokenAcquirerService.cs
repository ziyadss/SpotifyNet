using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Abstractions;

public interface ITokenAcquirerService
{
    Task EnsureTokenExists(
        IEnumerable<string> scopes,
        bool forceGenerate = false,
        CancellationToken cancellationToken = default);
}
