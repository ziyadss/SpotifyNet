using System;
using System.Runtime.Serialization;

namespace SpotifyNet.Core.Exceptions;

[Serializable]
internal class EnsureException : Exception
{
    public EnsureException()
    {
    }

    public EnsureException(string message)
        : base(message)
    {
    }

    public EnsureException(string message, Exception inner)
        : base(message, inner)
    {
    }

    protected EnsureException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
