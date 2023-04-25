using SpotifyNet.Services.Interfaces.WebAPI;

namespace SpotifyNet.Services.Interfaces;

public interface IWebAPIService
{
    IAlbumsService Albums { get; }

    IArtistsService Artists { get; }

    IPlayerService Player { get; }

    IPlaylistsService Playlists { get; }

    ITracksService Tracks { get; }

    IUsersService Users { get; }
}
