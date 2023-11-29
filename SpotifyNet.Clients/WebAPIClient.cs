using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Core.Exceptions;
using SpotifyNet.Core.Utilities;

namespace SpotifyNet.Clients;

public class WebAPIClient : IWebAPIClient
{
    private readonly HttpClient _httpClient;

    public WebAPIClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<TResponse> GetAsync<TResponse>(string uri, string accessToken, CancellationToken cancellationToken)
    {
        var method = HttpMethod.Get;

        return SendAsync<TResponse>(method, uri, accessToken, cancellationToken);
    }

    public Task PutAsync(string uri, string accessToken, CancellationToken cancellationToken)
    {
        var method = HttpMethod.Put;

        return SendAsync(method, uri, accessToken, cancellationToken);
    }

    public Task PutAsync<TPayload>(
        string uri,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var method = HttpMethod.Put;

        return SendAsync(method, uri, payload, accessToken, cancellationToken);
    }

    public Task DeleteAsync<TPayload>(
        string uri,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var method = HttpMethod.Delete;

        return SendAsync(method, uri, payload, accessToken, cancellationToken);
    }

    private async Task<TResponse> SendAsync<TResponse>(
        HttpMethod httpMethod,
        string uri,
        string accessToken,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(httpMethod, uri);
        request.Headers.Authorization = new("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        await Ensure.RequestSuccess(response, cancellationToken);

        try
        {
            var result = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);

            return result!;
        }
        catch (Exception e)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            throw new WebAPIException(uri, content, response.StatusCode, e);
        }
    }

    private async Task SendAsync<TPayload>(
        HttpMethod httpMethod,
        string uri,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken)
    {
        using var content = JsonContent.Create(payload);

        using var request = new HttpRequestMessage(httpMethod, uri);
        request.Content = content;

        request.Headers.Authorization = new("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        await Ensure.RequestSuccess(response, cancellationToken);
    }

    private async Task SendAsync(
        HttpMethod httpMethod,
        string uri,
        string accessToken,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(httpMethod, uri);

        request.Headers.Authorization = new("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        await Ensure.RequestSuccess(response, cancellationToken);
    }
}
