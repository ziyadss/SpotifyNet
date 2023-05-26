using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Clients.Interfaces;

public interface IWebAPIClient
{
    Task<TResponse> GetAsync<TResponse>(
        string uri,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task PutAsync(
        string uri,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task PutAsync<TPayload>(
        string uri,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task DeleteAsync<TPayload>(
        string uri,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken = default);
}
