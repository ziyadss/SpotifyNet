using System;
using System.Security.Cryptography;
using System.Text;

namespace SpotifyNet.Core.Utilities;

public static class PKCE
{
    private const int MinimumCodeVerifierLength = 43;
    private const int MaximumCodeVerifierLength = 128;

    public static (string Verifier, string Challenge) GetCodeVerifierAndChallenge(int verifierLength)
    {
        Ensure.Between(verifierLength, MinimumCodeVerifierLength, MaximumCodeVerifierLength, inclusive: true);

        var verifier = CodeVerifier(verifierLength);
        var challenge = CodeChallenge(verifier);

        return (verifier, challenge);
    }

    private static string CodeVerifier(int length)
    {
        Ensure.Between(length, MinimumCodeVerifierLength, MaximumCodeVerifierLength, inclusive: true);

        var verifierBytes = RandomNumberGenerator.GetBytes(length);
        var verifier = Base64UrlEncode(verifierBytes);

        return verifier;
    }

    private static string CodeChallenge(string verifier)
    {
        var challengeBytes = SHA256.HashData(Encoding.UTF8.GetBytes(verifier));
        var challenge = Base64UrlEncode(challengeBytes);

        return challenge;
    }

    private static string Base64UrlEncode(byte[] bytes) => Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
}
