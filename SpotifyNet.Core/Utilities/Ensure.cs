using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Core.Exceptions;

namespace SpotifyNet.Core.Utilities;

public static class Ensure
{
    public static void Equal<T>(T actual, T expected)
    {
        var equal = EqualityComparer<T>.Default.Equals(actual, expected);
        if (!equal)
        {
            throw new EnsureException($"Expected: `{expected}`, actual: `{actual}`.");
        }
    }

    public static void Between<T>(T actual, T minimum, T maximum, bool inclusive)
        where T : INumber<T>
    {
        if (inclusive)
        {
            if (actual < minimum || actual > maximum)
            {
                throw new EnsureException(
                    $"Expected: `{actual}` to be between `{minimum}` and `{maximum}`, inclusive.");
            }
        }
        else
        {
            if (actual <= minimum || actual >= maximum)
            {
                throw new EnsureException($"Expected: `{actual}` to be between `{minimum}` and `{maximum}`.");
            }
        }
    }

    public static void OneOf<T>(T actual, ICollection<T> expected)
    {
        if (!expected.Contains(actual))
        {
            throw new EnsureException($"Expected `{actual}` to be one of: `{string.Join(", ", expected)}`");
        }
    }

    public static async Task RequestSuccess(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(response, nameof(response));

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            var uri = response.RequestMessage?.RequestUri?.ToString() ?? string.Empty;
            throw new WebAPIException(uri, responseContent, response.StatusCode, e);
        }
    }
}
