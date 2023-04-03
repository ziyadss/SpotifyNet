using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Users;
using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

public class PlaylistTrack
{
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    [JsonPropertyName("added_by")]
    public User? AddedBy { get; init; }

    [JsonPropertyName("is_local")]
    public bool? IsLocal { get; init; }

    [JsonPropertyName("track")]
    public Track? Track { get; init; }
}
