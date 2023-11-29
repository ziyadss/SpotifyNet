using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Core.Utilities;
using SpotifyNet.Repositories.Abstractions;
using SpotifyNet.Services.Abstractions;

namespace SpotifyNet.Services;

public class TokenAcquirerService : ITokenAcquirerService
{
    private readonly IAuthorizationRepository _authorizationRepository;
    private readonly HttpListener _httpListener;

    public TokenAcquirerService(IAuthorizationRepository authorizationRepository, HttpListener httpListener)
    {
        _authorizationRepository = authorizationRepository;
        _httpListener = httpListener;
    }

    public async Task EnsureTokenExists(
        IEnumerable<string> scopes,
        bool forceGenerate,
        CancellationToken cancellationToken)
    {
        var scopesCollection = scopes.ToCollection();
        var needToGenerate = await NeedToGenerate(forceGenerate, scopesCollection, cancellationToken);

        if (needToGenerate)
        {
            await GenerateToken(scopesCollection, cancellationToken);
        }
    }

    private async Task<bool> NeedToGenerate(
        bool forceGenerate,
        ICollection<string> scopes,
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

        return missingScopes.Any();
    }

    private async Task GenerateToken(ICollection<string> scopes, CancellationToken cancellationToken)
    {
        var uri = await _authorizationRepository.GetUserAuthorizeUri(scopes, cancellationToken);

        var processInfo = new ProcessStartInfo
        {
            FileName = uri,
            UseShellExecute = true,
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
