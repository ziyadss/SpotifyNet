using SpotifyNet.Datastructures.Spotify.Tracks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces;

public interface IWebAPIService
{
    Task<IEnumerable<SavedTrack>> GetCurrentUserSavedTracks(
        CancellationToken cancellationToken = default);
}
