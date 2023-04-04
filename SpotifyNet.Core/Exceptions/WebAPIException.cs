using System;
using System.Runtime.Serialization;

namespace SpotifyNet.Core.Exceptions;

[Serializable]
public class WebAPIException : Exception
{
    public WebAPIException()
    {
    }

    public WebAPIException(string message)
        : base(message)
    {
    }

    public WebAPIException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected WebAPIException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
