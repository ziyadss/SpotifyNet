﻿using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Users;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces.WebAPI;

public interface IUsersService
{
    Task<User> GetCurrentUserProfile(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Track>> GetCurrentUserTopTracks(
        string timeRange = "medium_term",
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Artist>> GetCurrentUserTopArtists(
        string timeRange = "medium_term",
        CancellationToken cancellationToken = default);

    Task<User> GetUserProfile(
        string userId,
        CancellationToken cancellationToken = default);
}
