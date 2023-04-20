using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.SnippetDownloader;

internal interface ISnippetDownloader
{
    public Task<(string FileName, SnippetDownloadStatus Status)> DownloadTrack(
        string trackId,
        CancellationToken cancellationToken = default);

    public Task<IEnumerable<(string FileName, SnippetDownloadStatus Status)>> DownloadPlaylist(
        string playlistId,
        CancellationToken cancellationToken = default);
}
