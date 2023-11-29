using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.SnippetDownloader;

internal interface ISnippetDownloader
{
    Task<SnippetDownloadMetadata> DownloadTrack(string trackId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SnippetDownloadMetadata>> DownloadPlaylist(
        string playlistId,
        CancellationToken cancellationToken = default);
}
