using SpotifyNet.Auth;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.WebAPI;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyNet.Playground;

internal class Program
{
    private const string AppClientId = "";
    private const string AppRedirectUri = "http://localhost:3000";

    static async Task Main()
    {
        var newToken = false;
        var scopes = new[] { AuthorizationScope.PlaylistReadPrivate, AuthorizationScope.UserFollowRead };

        var authorizationClient = new AuthorizationClient(AppClientId, AppRedirectUri);
        var authorizationRepository = new AuthorizationRepository();
        var authorizationService = new AuthorizationService(authorizationClient, authorizationRepository);
        var tokenAcquirer = new TokenAcquirer(AppRedirectUri, authorizationService);

        string token;
        if (newToken)
        {
            token = await tokenAcquirer.GetToken(scopes, cancellationToken: default);
        }
        else
        {
            token = await tokenAcquirer.GetExistingToken(cancellationToken: default);
        }

        var webAPIClient = new WebAPIClient();
        var webAPIRepository = new WebAPIRepository(webAPIClient);

        var playlists = await webAPIRepository.GetCurrentUserPlaylists(token, ownerId: "ziyad.ss");

        Console.WriteLine(playlists[0].Name);
        Console.WriteLine(playlists[0].Tracks!.Total);

        var tracks = await webAPIRepository.GetPlaylistItems(token, playlists[0].Id!);
        Console.WriteLine(tracks.Count);
        Console.WriteLine(string.Join(',', tracks.Select(t => t.Track!.Name)));
    }
}
