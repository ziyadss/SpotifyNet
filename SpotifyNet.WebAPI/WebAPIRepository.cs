using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.WebAPI.Interfaces;
using System;
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
        string playlistId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetPlaylistItems(playlistId);

        var items = await GetPaginated<PlaylistTrack>(
            url,
            accessToken,
            cancellationToken);

        return items;
    }

    public async Task<IReadOnlyList<SavedTrack>> GetCurrentUserSavedTracks(
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        var url = Endpoints.GetUserSavedTracks();

        var tracks = await GetPaginated<SavedTrack>(
            url,
            accessToken,
            cancellationToken);

        return tracks;
    }

    public async Task<IReadOnlyList<AudioFeatures>> GetTracksAudioFeatures(
        string[] trackIds,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        Ensure.Between(trackIds.Length, 0, 100, inclusive: true);

        var url = Endpoints.GetTracksAudioFeatures(trackIds);

        var features = await _webAPIClient.GetAsync <AudioFeaturesSet>(url, accessToken, cancellationToken);

        return features.AudioFeatures;
    }

    //private async Task<List<T>> GetPaginated<T>(
    //    string initialUrl,
    //    string accessToken,
    //    CancellationToken cancellationToken)
    //{
    //    var items = new List<T>();

    //    var batch = await _webAPIClient.GetAsync<PaginationWrapper<T>>(initialUrl, accessToken, cancellationToken);
    //    items.AddRange(batch.Items);

    //    while (batch.Next is not null)
    //    {
    //        batch = await _webAPIClient.GetAsync<PaginationWrapper<T>>(batch.Next, accessToken, cancellationToken);
    //        items.AddRange(batch.Items);
    //    }

    //    return items;
    //}

    private async Task<List<T>> GetPaginated<T>(
        string initialUrl,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var items = new List<T>();

        var offset = 0;
        var limit = 50;
        var url = $"{initialUrl}?offset={offset}&limit={limit}";

        var batch = await _webAPIClient.GetAsync<PaginationWrapper<T>>(url, accessToken, cancellationToken);
        items.AddRange(batch.Items);

        while (batch.Items.Length != 0)
        {
            offset += 50;
            url = $"{initialUrl}?offset={offset}&limit={limit}";
            batch = await _webAPIClient.GetAsync<PaginationWrapper<T>>(url, accessToken, cancellationToken);
            items.AddRange(batch.Items);

            Console.WriteLine($"{items.Count}/{batch.Total}");
        }

        return items;
    }
}
