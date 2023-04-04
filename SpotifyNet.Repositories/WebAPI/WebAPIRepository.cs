using SpotifyNet.Clients.Interfaces;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository : IWebAPIRepository
{
    private readonly IWebAPIClient _webAPIClient;

    public WebAPIRepository(IWebAPIClient webAPIClient)
    {
        _webAPIClient = webAPIClient;
    }

    private async Task<IEnumerable<T>> GetPaginated<T>(
        string initialUri,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var batch = await _webAPIClient.GetAsync<PaginationWrapper<T>>(
            initialUri,
            accessToken,
            cancellationToken);

        var items = new List<T>(batch.Total);
        items.AddRange(batch.Items);

        while (batch.Next is not null)
        {
            batch = await _webAPIClient.GetAsync<PaginationWrapper<T>>(
                batch.Next,
                accessToken,
                cancellationToken);

            items.AddRange(batch.Items);
        }

        return items;
    }
}
