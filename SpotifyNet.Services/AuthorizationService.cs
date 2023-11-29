using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Core.Exceptions;
using SpotifyNet.Repositories.Abstractions;
using SpotifyNet.Services.Abstractions;

namespace SpotifyNet.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationRepository _authorizationRepository;

    public AuthorizationService(IAuthorizationRepository authorizationRepository)
    {
        _authorizationRepository = authorizationRepository;
    }

    public async Task<string> GetAccessToken(IEnumerable<string> scopes, CancellationToken cancellationToken)
    {
        var token = await _authorizationRepository.GetAccessToken(cancellationToken);

        var missingScopes = scopes.Except(token.AuthorizationScopes).ToList();

        if (missingScopes.Count != 0)
        {
            throw new AuthorizationException(
                $"The current access token does not have the required authorization scopes `{string.Join(", ", missingScopes)}`.");
        }

        return token.Token;
    }
}
