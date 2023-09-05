namespace SpotifyNet.SnippetDownloader;

internal class SnippetDownloadMetadata
{
    public required string TrackId { get; init; }

    public required string FileName { get; init; }

    public required SnippetDownloadStatus Status { get; init; }
}
