using System.Text.Json.Serialization;

namespace SpotifyNet.SnippetDownloader;

[JsonConverter(typeof(JsonStringEnumConverter))]
enum SnippetDownloadStatus
{
    Unknown = 0,
    Downloaded,
    NoPreviewUrl,
    Failed,
    Exists,
};
