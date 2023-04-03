using SpotifyNet.Auth.Interfaces;
using SpotifyNet.Datastructures.Internal;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Auth;

public class AuthorizationRepository : IAuthorizationRepository
{
    private const string AuthorizationMetadataFilePath = "AuthorizationMetadata.json";
    private const string AccessTokenFilePath = "AccessToken.json";

    public AuthorizationRepository()
    {
    }

    public async Task<UserAuthorizationMetadata> ReadAuthorizationMetadata(CancellationToken cancellationToken)
    {
        using var fs = File.OpenRead(AuthorizationMetadataFilePath);
        var metadata = await JsonSerializer.DeserializeAsync<UserAuthorizationMetadata>(fs, cancellationToken: cancellationToken);
        return metadata!;
    }

    public Task WriteAuthorizationMetadata(UserAuthorizationMetadata userAuthorizationMetadata, CancellationToken cancellationToken)
    {
        File.Delete(AuthorizationMetadataFilePath);
        using var fs = File.OpenWrite(AuthorizationMetadataFilePath);
        return JsonSerializer.SerializeAsync(fs, userAuthorizationMetadata, cancellationToken: cancellationToken);
    }

    public async Task<AccessTokenMetadata> ReadAccessToken(CancellationToken cancellationToken)
    {
        using var fs = File.OpenRead(AccessTokenFilePath);
        var metadata = await JsonSerializer.DeserializeAsync<AccessTokenMetadata>(fs, cancellationToken: cancellationToken);
        return metadata!;
    }

    public Task WriteAccessToken(AccessTokenMetadata accessTokenMetadata, CancellationToken cancellationToken)
    {
        File.Delete(AccessTokenFilePath);
        using var fs = File.OpenWrite(AccessTokenFilePath);
        return JsonSerializer.SerializeAsync(fs, accessTokenMetadata, cancellationToken: cancellationToken);
    }

    public Task<bool> AccessTokenExists(CancellationToken cancellationToken)
    {
        var exists = File.Exists(AccessTokenFilePath);

        return Task.FromResult(exists);
    }
}
