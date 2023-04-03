using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Albums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AlbumType
{
    album,
    single,
    compilation,
}
