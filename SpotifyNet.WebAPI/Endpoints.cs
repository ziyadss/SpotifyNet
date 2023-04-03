using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyNet.WebAPI;

internal static class Endpoints
{
    public static string GetCurrentUserPlaylists() => $"https://api.spotify.com/v1/me/playlists";
    public static string GetPlaylistItems(string playlistId) => $"https://api.spotify.com/v1/playlists/{playlistId}/tracks";
}
