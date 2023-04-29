using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces;

public interface IAuthorizationService
{
    Task<string> GetAccessToken(
        IEnumerable<string> scopes,
        CancellationToken cancellationToken = default);
}
