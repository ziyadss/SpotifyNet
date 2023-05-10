using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Tracks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces.WebAPI;

public interface IAlbumsService
{
    Task<Album> GetAlbum(
        string albumId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Album>> GetAlbums(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SimplifiedTrack>> GetTracks(
        string albumId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SavedAlbum>> GetUserSavedAlbums(
        CancellationToken cancellationToken = default);

    Task SaveAlbums(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken = default);

    Task UnsaveAlbums(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<bool>> AreAlbumsSaved(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SimplifiedAlbum>> GetNewReleases(
        CancellationToken cancellationToken = default);
}
