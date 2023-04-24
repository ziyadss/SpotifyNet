using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.Services.Interfaces.WebAPI;

namespace SpotifyNet.Services.WebAPI;

public class WebAPIService : IWebAPIService
{
    public IAlbumsService Albums { get; init; }

    public IUsersService Users { get; init; }

    public ITracksService Tracks { get; init; }

    public IPlaylistsService Playlists { get; init; }

    public WebAPIService(
        IAuthorizationService authorizationService,
        IWebAPIRepository webAPIRepository)
    {
        Albums = new AlbumsService(authorizationService, webAPIRepository);
        Users = new UsersService(authorizationService, webAPIRepository);
        Tracks = new TracksService(authorizationService, webAPIRepository);
        Playlists = new PlaylistsService(authorizationService, webAPIRepository);
    }
}
