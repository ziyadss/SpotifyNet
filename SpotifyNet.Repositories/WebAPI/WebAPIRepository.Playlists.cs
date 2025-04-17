using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Playlists;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    public Task<Playlist> GetPlaylist(
        string playlistId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetPlaylist(playlistId);

        return _webAPIClient.GetAsync<Playlist>(uri, accessToken, cancellationToken);
    }

    public Task<IReadOnlyList<SimplifiedPlaylist>> GetCurrentUserPlaylists(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetCurrentUserPlaylists();

        return GetOffsetPaginated<SimplifiedPlaylist>(uri, accessToken, cancellationToken);
    }

    public Task<IReadOnlyList<PlaylistTrack>> GetPlaylistItems(
        string playlistId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetPlaylistItems(playlistId);

        return GetOffsetPaginated<PlaylistTrack>(uri, accessToken, cancellationToken);
    }

    public Task<IReadOnlyList<SimplifiedPlaylist>> GetUserPlaylists(
        string userId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetUserPlaylists(userId);

        return GetOffsetPaginated<SimplifiedPlaylist>(uri, accessToken, cancellationToken);
    }

    public Task AddCustomPlaylistCoverImage(
        string playlistId,
        string base64Image,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.AddCustomPlaylistCoverImage(playlistId);

        return _webAPIClient.PutAsync(
            uri,
            base64Image,
            MediaTypeNames.Image.Jpeg,
            accessToken,
            cancellationToken);
    }
}
