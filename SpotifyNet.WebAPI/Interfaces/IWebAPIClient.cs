using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.WebAPI.Interfaces;

public interface IWebAPIClient
{
    Task<T> GetAsync<T>(
        string url,
        string accessToken,
        CancellationToken cancellationToken = default);
}
