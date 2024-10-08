﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface ITracksService
{
    Task<IReadOnlyList<SavedTrack>> GetCurrentUserSavedTracks(CancellationToken cancellationToken = default);

    Task<Track> GetTrack(string trackId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<bool>> AreTracksSaved(
        IEnumerable<string> trackids,
        CancellationToken cancellationToken = default);
}
