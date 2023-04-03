using SpotifyNet.Auth.Interfaces;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Playground;

internal class TokenAcquirer : ITokenAcquirer
{
    private readonly IAuthorizationService _authorizationService;
    private readonly HttpListener _httpListener;

    public TokenAcquirer(
        string appRedirectUri,
        IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _httpListener = new();

        if (appRedirectUri.EndsWith('/'))
        {
            _httpListener.Prefixes.Add(appRedirectUri);
        }
        else
        {
            _httpListener.Prefixes.Add(appRedirectUri + '/');
        }
    }

    public async Task<string> GetToken(string[] scopes, CancellationToken cancellationToken)
    {
        var url = await _authorizationService.GetUserAuthorizeUrl(scopes, cancellationToken);

        var processInfo = new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        };
        Process.Start(processInfo);

        _httpListener.Start();
        var context = await _httpListener.GetContextAsync();
        var query = context.Request.QueryString;

        var code = query["code"]!;
        var state = query["state"]!;
        var token = await _authorizationService.GetNewAccessToken(code, state, cancellationToken);

        _httpListener.Stop();
        return token;
    }

    public Task<string> GetExistingToken(CancellationToken cancellationToken)
    {
        return _authorizationService.GetExistingAccessToken(cancellationToken);
    }
}
