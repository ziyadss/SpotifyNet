﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Users;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    private readonly string[] _validTimeRanges =
    {
        "long_term",
        "medium_term",
        "short_term",
    };

    public Task<User> GetCurrentUserProfile(string accessToken, CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetCurrentUserProfile();

        return _webAPIClient.GetAsync<User>(uri, accessToken, cancellationToken);
    }

    public Task<IReadOnlyList<Track>> GetCurrentUserTopTracks(
        string timeRange,
        string accessToken,
        CancellationToken cancellationToken)
    {
        Ensure.OneOf(timeRange, _validTimeRanges);

        var uri = Endpoints.GetCurrentUserTopItems("tracks", timeRange);

        return GetOffsetPaginated<Track>(uri, accessToken, cancellationToken);
    }

    public Task<IReadOnlyList<Artist>> GetCurrentUserTopArtists(
        string timeRange,
        string accessToken,
        CancellationToken cancellationToken)
    {
        Ensure.OneOf(timeRange, _validTimeRanges);

        var uri = Endpoints.GetCurrentUserTopItems("artists", timeRange);

        return GetOffsetPaginated<Artist>(uri, accessToken, cancellationToken);
    }

    public Task<User> GetUserProfile(string userId, string accessToken, CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetUserProfile(userId);

        return _webAPIClient.GetAsync<User>(uri, accessToken, cancellationToken);
    }
}
