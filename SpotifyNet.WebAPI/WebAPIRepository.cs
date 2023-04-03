using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.WebAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.WebAPI;

public class WebAPIRepository : IWebAPIRepository
{
    private readonly IWebAPIClient _webAPIClient;

    public WebAPIRepository(IWebAPIClient webAPIClient)
    {
        _webAPIClient = webAPIClient;
    }

    public async Task<IReadOnlyList<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        string accessToken,
        string? ownerId,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetCurrentUserPlaylists();

        var playlists = await GetPaginated<SimplifiedPlaylist>(
            url,
            accessToken,
            cancellationToken);

        if (ownerId is null)
        {
            return playlists;
        }
        else
        {
            return playlists.Where(p => p.Owner?.Id == ownerId).ToList();
        }
    }

    public async Task<IReadOnlyList<PlaylistTrack>> GetPlaylistItems(
        string accessToken,
        string playlistId,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetPlaylistItems(playlistId);

        var items = await GetPaginated<PlaylistTrack>(
            url,
            accessToken,
            cancellationToken);

        return items;
    }

    private async Task<List<T>> GetPaginated<T>(
        string initialUrl,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var items = new List<T>();

        var batch = await _webAPIClient.GetAsync<PaginationWrapper<T>>(initialUrl, accessToken, cancellationToken);
        items.AddRange(batch.Items);

        while (batch.Next is not null)
        {
            batch = await _webAPIClient.GetAsync<PaginationWrapper<T>>(batch.Next, accessToken, cancellationToken);
            items.AddRange(batch.Items);
        }

        return items;
    }
}
