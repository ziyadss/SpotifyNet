using SpotifyNet.Core.Utilities;
using SpotifyNet.WebAPI.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.WebAPI;

public class WebAPIClient : IWebAPIClient
{
    private readonly HttpClient _httpClient;

    public WebAPIClient()
    {
        _httpClient = new();
    }

    public Task<TResponse> GetAsync<TResponse>(
        string url,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var method = HttpMethod.Get;

        return SendAsync<TResponse>(
            method,
            url,
            accessToken,
            HttpStatusCode.OK,
            cancellationToken);
    }

    public Task PutAsync<TPayload>(
        string url,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var method = HttpMethod.Put;

        return SendAsync(
            method,
            url,
            payload,
            accessToken,
            HttpStatusCode.OK,
            cancellationToken);
    }

    public Task DeleteAsync<TPayload>(
        string url,
        TPayload payload,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var method = HttpMethod.Delete;

        return SendAsync(
            method,
            url,
            payload,
            accessToken,
            HttpStatusCode.OK,
            cancellationToken);
    }

    private async Task<TResponse> SendAsync<TResponse>(
        HttpMethod httpMethod,
        string url,
        string accessToken,
        HttpStatusCode expectedStatusCode,
        CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(httpMethod, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        await Ensure.RequestSuccess(response, expectedStatusCode);

        try
        {
            var result = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);

            return result!;
        }
        catch (Exception ex)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception(
                message: $"Failed to deserialize response from `{url}`. Response: `{content}`",
                innerException: ex);
        }
    }

    private async Task SendAsync<TPayload>(
        HttpMethod httpMethod,
        string url,
        TPayload payload,
        string accessToken,
        HttpStatusCode expectedStatusCode,
        CancellationToken cancellationToken)
    {
        using var content = JsonContent.Create(payload);

        var request = new HttpRequestMessage(httpMethod, url)
        {
            Content = content
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        await Ensure.RequestSuccess(response, expectedStatusCode);
    }
}
