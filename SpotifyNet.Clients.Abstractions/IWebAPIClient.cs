using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Clients.Abstractions;

public interface IWebAPIClient
{
    Task<TResponse> GetAsync<TResponse>(string uri, string accessToken, CancellationToken cancellationToken = default);

    Task PutAsync(string uri, string accessToken, CancellationToken cancellationToken = default);

    Task PutAsync<TPayload>(
        string uri,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken = default);

    public Task PutAsync(
        string uri,
        string payload,
        string contentType,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task DeleteAsync<TPayload>(
        string uri,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken = default);
}
