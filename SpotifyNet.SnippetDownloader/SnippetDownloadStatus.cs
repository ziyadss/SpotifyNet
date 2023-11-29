using System.Text.Json.Serialization;

namespace SpotifyNet.SnippetDownloader;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum SnippetDownloadStatus
{
    Unknown = 0,
    Downloaded,
    NoPreviewUrl,
    Failed,
    Exists,
    LocalFile,
}
