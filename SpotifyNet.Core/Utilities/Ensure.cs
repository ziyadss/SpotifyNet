using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
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

    public static void Between<T>(T actual, T minimum, T maximum, bool inclusive = false)
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

    public static async Task RequestSuccess(
        HttpResponseMessage response,
        HttpStatusCode expectedStatusCode)
    {
        try
        {
            Equal(response.StatusCode, expectedStatusCode);
        }
        catch (EnsureException)
        {
            var content = await response.Content.ReadAsStringAsync();
            var errorMessage = $"Request failed with status code `{response.StatusCode}`. Content: `{content}`";
            throw new EnsureException(errorMessage);
        }
    }
}
