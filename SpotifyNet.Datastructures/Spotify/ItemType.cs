using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemType
{
    album,
    artist,
    track,
    playlist,
    user,
    audio_features,
}
