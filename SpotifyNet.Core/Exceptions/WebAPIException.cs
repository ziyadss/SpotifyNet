using System;
using System.Net;

namespace SpotifyNet.Core.Exceptions;

public class WebAPIException : Exception
{
    public WebAPIException(string uri, string content, HttpStatusCode statusCode, Exception innerException)
        : base(BuildMessage(uri, content, statusCode), innerException)
    {
    }

    private static string BuildMessage(
        string uri,
        string content,
        HttpStatusCode statusCode)
    {
        return $"Parsing response from Spotify Web API failed.\n" +
               $"URI: {uri}\n" +
               $"Status code: {statusCode}\n" +
               $"Content: {content}";
    }
}
