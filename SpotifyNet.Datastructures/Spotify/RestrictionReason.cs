using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RestrictionReason
{
    market,
    product,
    @explicit,
    payment_required,
}
