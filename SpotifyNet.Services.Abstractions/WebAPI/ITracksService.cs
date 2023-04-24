using SpotifyNet.Datastructures.Spotify.Tracks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces.WebAPI;

public interface ITracksService
{
    Task<IEnumerable<SavedTrack>> GetCurrentUserSavedTracks(
        CancellationToken cancellationToken = default);

    Task<Track> GetTrack(
        string trackId,
        CancellationToken cancellationToken = default);
}
