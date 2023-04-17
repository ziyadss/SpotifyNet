using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyNet.SnippetDownloader;

internal interface ISnippetDownloader
{
    public Task<(string FileName, SnippetDownloadStatus Status)> DownloadTrack(string trackId);

    public Task<IEnumerable<(string FileName, SnippetDownloadStatus Status)>> DownloadPlaylist(string playlistId);
}
