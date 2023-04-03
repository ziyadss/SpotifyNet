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

internal class WebAPIClient : IWebAPIClient
{
    private readonly HttpClient _httpClient;

    public WebAPIClient()
    {
        _httpClient = new();
    }

    public async Task<T> GetAsync<T>(
        string url,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var method = HttpMethod.Get;

        var result = await SendAsync<T>(
            method,
            url,
            accessToken,
            HttpStatusCode.OK,
            cancellationToken);

        return result;
    }

    private async Task<T> SendAsync<T>(
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
            var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);

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
}
