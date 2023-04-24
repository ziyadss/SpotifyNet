using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.Services.Interfaces.WebAPI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.WebAPI;

public class AlbumsService : IAlbumsService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebAPIRepository _webAPIRepository;

    public AlbumsService(
        IAuthorizationService authorizationService,
        IWebAPIRepository webAPIRepository)
    {
        _authorizationService = authorizationService;
        _webAPIRepository = webAPIRepository;
    }

    public async Task<Album> GetAlbum(
        string albumId,
        CancellationToken cancellationToken)
    {
        var requiredScopes = Array.Empty<string>();

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var album = await _webAPIRepository.GetAlbum(albumId, accessToken, cancellationToken);

        return album;
    }
}
