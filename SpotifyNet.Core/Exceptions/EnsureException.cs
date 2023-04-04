using System;
using System.Runtime.Serialization;

namespace SpotifyNet.Core.Exceptions;

[Serializable]
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

    protected EnsureException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
