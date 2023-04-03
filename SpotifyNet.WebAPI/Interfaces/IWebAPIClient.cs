using System.Threading.Tasks;
using System.Threading;

namespace SpotifyNet.WebAPI.Interfaces;

internal interface IWebAPIClient
{
    Task<T> GetAsync<T>(
        string url,
        string accessToken,
        CancellationToken cancellationToken);
}
