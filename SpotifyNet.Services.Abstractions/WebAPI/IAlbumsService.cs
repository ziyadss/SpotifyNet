using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IAlbumsService
{
    Task<Album> GetAlbum(string albumId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Album>> GetAlbums(IEnumerable<string> albumIds, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SimplifiedTrack>> GetTracks(string albumId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SavedAlbum>> GetUserSavedAlbums(CancellationToken cancellationToken = default);

    Task SaveAlbums(IEnumerable<string> albumIds, CancellationToken cancellationToken = default);

    Task RemoveAlbums(IEnumerable<string> albumIds, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<bool>> AreAlbumsSaved(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SimplifiedAlbum>> GetNewReleases(CancellationToken cancellationToken = default);
}
