using SpotifyNet.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
        where T : IComparable
    {
        if (inclusive)
        {
            if (actual.CompareTo(minimum) < 0 || actual.CompareTo(maximum) > 0)
            {
                throw new EnsureException($"Expected: `{actual}` to be between `{minimum}` and `{maximum}`, inclusive.");
            }
        }
        else
        {
            if (actual.CompareTo(minimum) <= 0 || actual.CompareTo(maximum) >= 0)
            {
                throw new EnsureException($"Expected: `{actual}` to be between `{minimum}` and `{maximum}`.");
            }
        }
    }

    public static void OneOf<T>(T actual, IEnumerable<T> expected)
    {
        if (!expected.Contains(actual))
        {
            throw new EnsureException($"Expected `{actual}` to be one of: `{string.Join(", ", expected)}`");
        }
    }

    public static async Task RequestSuccess(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(response, nameof(response));

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            throw new WebAPIException(
                message: $"Request failed with status code `{response.StatusCode}`. Content: `{responseContent}`",
                innerException: ex);
        }
    }
}
