using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Albums;

public record AlbumsSet
{
    [JsonPropertyName("albums")]
    public required Album[] Albums { get; init; }
}
