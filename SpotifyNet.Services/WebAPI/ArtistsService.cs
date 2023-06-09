﻿using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
using SpotifyNet.Services.Interfaces.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

    public async Task<Artist> GetArtist(
        string artistId,
        CancellationToken cancellationToken)
    {
        var requiredScopes = Array.Empty<string>();

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var artist = await _webAPIRepository.GetArtist(artistId, accessToken, cancellationToken);

        return artist;
    }

    public async Task<IReadOnlyList<Artist>> GetArtists(
        IEnumerable<string> artistIds,
        CancellationToken cancellationToken)
    {
        var requiredScopes = Array.Empty<string>();

        var accessToken = await _authorizationService.GetAccessToken(requiredScopes, cancellationToken);

        var artistIdsCollection = artistIds as ICollection<string> ?? artistIds.ToList();
        var artists = new List<Artist>(artistIdsCollection.Count);

        foreach (var chunk in artistIdsCollection.Chunk(50))
        {
            var batch = await _webAPIRepository.GetArtists(chunk, accessToken, cancellationToken);
            artists.AddRange(batch);
        }

        return artists;
    }
}
