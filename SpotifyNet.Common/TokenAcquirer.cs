using SpotifyNet.Repositories.Interfaces;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Common;

public class TokenAcquirer : ITokenAcquirer
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

    public async Task EnsureTokenExists(
        string[] scopes,
        bool forceGenerate,
        CancellationToken cancellationToken)
    {
        var needToGenerate = await NeedToGenerate(forceGenerate, scopes, cancellationToken);

        if (needToGenerate)
        {
            await GenerateToken(scopes, cancellationToken);
        }
    }

    private async Task<bool> NeedToGenerate(
        bool forceGenerate,
        string[] scopes,
        CancellationToken cancellationToken)
    {
        if (forceGenerate)
        {
            return true;
        }

        var exists = await _authorizationRepository.AccessTokenExists(cancellationToken);
        if (!exists)
        {
            return true;
        }

        var accessToken = await _authorizationRepository.GetAccessToken(cancellationToken);
        var missingScopes = scopes.Except(accessToken.AuthorizationScopes);
        if (missingScopes.Any())
        {
            return true;
        }

        return false;
    }

    private async Task GenerateToken(
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
