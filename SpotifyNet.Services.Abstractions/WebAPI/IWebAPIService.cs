using SpotifyNet.Services.Interfaces.WebAPI;

namespace SpotifyNet.Services.Interfaces;

public interface IWebAPIService
{
    IAlbumsService Albums { get; init; }

    IUsersService Users { get; init; }

    ITracksService Tracks { get; init; }

    IPlaylistsService Playlists { get; init; }
}
