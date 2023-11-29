using SpotifyNet.Repositories.Abstractions;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

public class WebAPIService : IWebAPIService
{
    public WebAPIService(IAuthorizationService authorizationService, IWebAPIRepository webAPIRepository)
    {
        Albums = new AlbumsService(authorizationService, webAPIRepository);
        Artists = new ArtistsService(authorizationService, webAPIRepository);
        Player = new PlayerService(authorizationService, webAPIRepository);
        Playlists = new PlaylistsService(authorizationService, webAPIRepository);
        Tracks = new TracksService(authorizationService, webAPIRepository);
        Users = new UsersService(authorizationService, webAPIRepository);
    }

    public IAlbumsService Albums { get; }

    public IArtistsService Artists { get; }

    public IPlayerService Player { get; }

    public IPlaylistsService Playlists { get; }

    public ITracksService Tracks { get; }

    public IUsersService Users { get; }
}
