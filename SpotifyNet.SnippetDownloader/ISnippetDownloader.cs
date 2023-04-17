using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyNet.SnippetDownloader;

internal interface ISnippetDownloader
{
    public Task<(string, SnippetDownloadStatus)> DownloadTrack(string trackId);

    public Task<IEnumerable<(string TrackName, SnippetDownloadStatus Status)>> DownloadPlaylist(string playlistId);
}
