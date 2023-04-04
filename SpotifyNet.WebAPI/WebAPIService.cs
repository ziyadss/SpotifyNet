using SpotifyNet.WebAPI.Interfaces;

namespace SpotifyNet.WebAPI;

public class WebAPIService : IWebAPIService
{
    private readonly IWebAPIRepository _repository;

    public WebAPIService(IWebAPIRepository repository)
    {
        _repository = repository;
    }
}
