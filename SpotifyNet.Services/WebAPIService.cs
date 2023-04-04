using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;

namespace SpotifyNet.WebAPI;

public class WebAPIService : IWebAPIService
{
    private readonly IWebAPIRepository _webAPIRepository;

    public WebAPIService(IWebAPIRepository webAPIRepository)
    {
        _webAPIRepository = webAPIRepository;
    }
}
