using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Albums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AlbumGroup
{
    album,
    single,
    compilation,
    appears_on,
}
