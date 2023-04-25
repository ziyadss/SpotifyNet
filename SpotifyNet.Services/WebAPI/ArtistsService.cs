using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.Services.Interfaces.WebAPI;

namespace SpotifyNet.Services.WebAPI;

public class ArtistsService : IArtistsService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebAPIRepository _webAPIRepository;

    public ArtistsService(
        IAuthorizationService authorizationService,
        IWebAPIRepository webAPIRepository)
    {
        _authorizationService = authorizationService;
        _webAPIRepository = webAPIRepository;
    }
}
