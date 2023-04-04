using SpotifyNet.Clients.Interfaces;
using SpotifyNet.Core.Exceptions;
using SpotifyNet.Core.Utilities;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Clients.WebAPI;

public class WebAPIClient : IWebAPIClient
{
    private readonly HttpClient _httpClient;

    public WebAPIClient()
    {
        _httpClient = new();
    }

    public Task<TResponse> GetAsync<TResponse>(
        string uri,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var method = HttpMethod.Get;

        return SendAsync<TResponse>(
            method,
            uri,
            accessToken,
            cancellationToken);
    }

    public Task PutAsync<TPayload>(
        string uri,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var method = HttpMethod.Put;

        return SendAsync(
            method,
            uri,
            payload,
            accessToken,
            cancellationToken);
    }

    public Task DeleteAsync<TPayload>(
        string uri,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var method = HttpMethod.Delete;

        return SendAsync(
            method,
            uri,
            payload,
            accessToken,
            cancellationToken);
    }

    private async Task<TResponse> SendAsync<TResponse>(
        HttpMethod httpMethod,
        string uri,
        string accessToken,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(httpMethod, uri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        await Ensure.RequestSuccess(response, cancellationToken);

        try
        {
            var result = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);

            return result!;
        }
        catch (Exception ex)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new WebAPIException(
                message: $"Failed to deserialize response from `{uri}`. Response: `{content}`",
                innerException: ex);
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

        using var request = new HttpRequestMessage(httpMethod, uri)
        {
            Content = content
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        await Ensure.RequestSuccess(response, cancellationToken);
    }
}
