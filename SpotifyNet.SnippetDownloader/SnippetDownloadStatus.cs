namespace SpotifyNet.SnippetDownloader;

enum SnippetDownloadStatus
{
    Unknown = 0,
    Downloaded,
    NoPreviewUrl,
    Failed,
    Exists,
};
