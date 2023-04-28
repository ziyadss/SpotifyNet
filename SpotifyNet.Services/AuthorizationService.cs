using SpotifyNet.Core.Exceptions;
using SpotifyNet.Repositories.Interfaces;
using SpotifyNet.Services.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Authorization;

public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationRepository _authorizationRepository;

    public AuthorizationService(
        IAuthorizationRepository authorizationRepository)
    {
        _authorizationRepository = authorizationRepository;
    }

    public async Task<string> GetAccessToken(
        string[] scopes,
        CancellationToken cancellationToken)
    {
        var token = await _authorizationRepository.GetAccessToken(cancellationToken);

        var missingScopes = scopes.Except(token.AuthorizationScopes);

        if (missingScopes.Any())
        {
            throw new AuthorizationException(
                $"The current access token does not have the required authorization scopes `{string.Join(", ", missingScopes)}`.");
        }

        return token.Token;
    }
}
