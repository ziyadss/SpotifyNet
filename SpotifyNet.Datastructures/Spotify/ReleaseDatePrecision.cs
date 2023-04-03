using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReleaseDatePrecision
{
    year,
    month,
    day,
}
