using SpotifyNet.Datastructures.Spotify.Albums;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Interfaces.WebAPI;

public interface IAlbumsService
{
    Task<Album> GetAlbum(
        string albumId,
        CancellationToken cancellationToken = default);
}
