using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Clients.Interfaces;

public interface IWebAPIClient
{
    Task<TResponse> GetAsync<TResponse>(
        string url,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task PutAsync<TPayload>(
        string url,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task DeleteAsync<TPayload>(
        string url,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken = default);
}
