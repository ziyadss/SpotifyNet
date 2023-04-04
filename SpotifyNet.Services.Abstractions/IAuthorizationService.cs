using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces;

public interface IAuthorizationService
{
    Task<string> GetAccessToken(
        string[] scopes,
        CancellationToken cancellationToken = default);
}
