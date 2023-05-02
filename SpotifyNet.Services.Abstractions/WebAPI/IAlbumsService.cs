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

    Task<IEnumerable<Album>> GetAlbums(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SimplifiedTrack>> GetTracks(
        string albumId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SavedAlbum>> GetUserSavedAlbums(
        CancellationToken cancellationToken = default);

    Task SaveAlbums(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken = default);

    Task UnsaveAlbums(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<bool>> AreAlbumsSaved(
        IEnumerable<string> albumIds,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SimplifiedAlbum>> GetNewReleases(
        CancellationToken cancellationToken = default);
}
