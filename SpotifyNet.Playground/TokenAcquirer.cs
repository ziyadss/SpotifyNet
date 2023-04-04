using SpotifyNet.Repositories.Interfaces;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Playground;

internal class TokenAcquirer : ITokenAcquirer
{
    private readonly IAuthorizationRepository _authorizationRepository;
    private readonly HttpListener _httpListener;

    public TokenAcquirer(
        IAuthorizationRepository authorizationRepository,
        HttpListener httpListener)
    {
        _authorizationRepository = authorizationRepository;
        _httpListener = httpListener;
    }

    public async Task GenerateToken(
        string[] scopes,
        CancellationToken cancellationToken)
    {
        var uri = await _authorizationRepository.GetUserAuthorizeUri(scopes, cancellationToken);

        var processInfo = new ProcessStartInfo
        {
            FileName = uri,
            UseShellExecute = true
        };

        Process.Start(processInfo);

        _httpListener.Start();
        var context = await _httpListener.GetContextAsync();
        var query = context.Request.QueryString;

        var code = query["code"]!;
        var state = query["state"]!;
        await _authorizationRepository.GetNewAccessToken(code, state, cancellationToken);

        _httpListener.Stop();
    }
}
