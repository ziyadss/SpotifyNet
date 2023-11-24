using System;

namespace SpotifyNet.Core.Exceptions;

public class EnsureException : Exception
{
    public EnsureException()
    {
    }

    public EnsureException(string message)
        : base(message)
    {
    }

    public EnsureException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
