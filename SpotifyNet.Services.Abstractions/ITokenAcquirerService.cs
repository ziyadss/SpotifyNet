using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces;

public interface ITokenAcquirerService
{
    Task EnsureTokenExists(
        string[] scopes,
        bool forceGenerate = false,
        CancellationToken cancellationToken = default);
}
