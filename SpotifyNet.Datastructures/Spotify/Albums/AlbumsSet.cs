using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Albums;

public class AlbumsSet
{
    [JsonPropertyName("albums")]
    public required Album[] Albums { get; init; }
}
