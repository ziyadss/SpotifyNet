using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Repositories.Abstractions;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

public class ArtistsService : IArtistsService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebAPIRepository _webAPIRepository;

    public ArtistsService(IAuthorizationService authorizationService, IWebAPIRepository webAPIRepository)
    {
        _authorizationService = authorizationService;
        _webAPIRepository = webAPIRepository;
    }

    public async Task<Artist> GetArtist(string artistId, CancellationToken cancellationToken)
    {
        var accessToken = await _authorizationService.GetAccessToken(cancellationToken);

        var artist = await _webAPIRepository.GetArtist(artistId, accessToken, cancellationToken);

        return artist;
    }

    public Task<IReadOnlyList<Artist>> GetArtists(IEnumerable<string> artistIds, CancellationToken cancellationToken) =>
        GetArtistsOne(artistIds, cancellationToken);

    public async Task<IReadOnlyList<Artist>> GetArtistsOne(
        IEnumerable<string> artistIds,
        CancellationToken cancellationToken)
    {
        var accessToken = await _authorizationService.GetAccessToken(cancellationToken);

        var artistIdsCollection = artistIds.ToCollection();
        var artists = new List<Artist>(artistIdsCollection.Count);

        foreach (var chunk in artistIdsCollection.Chunk(50))
        {
            var batch = await _webAPIRepository.GetArtists(chunk, accessToken, cancellationToken);
            artists.AddRange(batch);
        }

        return artists;
    }

    public async Task<IReadOnlyList<Artist>> GetArtistsTwo(
        IEnumerable<string> artistIds,
        CancellationToken cancellationToken)
    {
        var accessToken = await _authorizationService.GetAccessToken(cancellationToken);

        var artistIdsCollection = artistIds.ToCollection();

        var artists = await artistIdsCollection
                           .ChunkedSelect(
                                50, chunk => _webAPIRepository.GetArtists(chunk, accessToken, cancellationToken));

        return artists;
    }
}
