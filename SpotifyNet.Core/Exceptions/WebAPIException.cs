using System;

namespace SpotifyNet.Core.Exceptions;

public class WebAPIException : Exception
{
    public WebAPIException(string message)
        : base(message)
    {
    }

    public WebAPIException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
