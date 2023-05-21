using SpotifyNet.Datastructures.Spotify.Tracks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces.WebAPI;

public interface ITracksService
{
    Task<IReadOnlyList<SavedTrack>> GetCurrentUserSavedTracks(
        CancellationToken cancellationToken = default);

    Task<Track> GetTrack(
        string trackId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<bool>> AreTracksSaved(
        IEnumerable<string> trackids,
        CancellationToken cancellationToken = default);
}
