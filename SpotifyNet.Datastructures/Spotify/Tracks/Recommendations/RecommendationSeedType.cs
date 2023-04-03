using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks.Recommendations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RecommendationSeedType
{
    artist,
    track,
    genre,
}
