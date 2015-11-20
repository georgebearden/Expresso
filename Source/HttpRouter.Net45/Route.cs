using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace HttpRouter
{
  /// <summary>
  /// Routes a http request endpoint to callback.
  /// </summary>
  public class Route
  {
    // holds the set of resource ids
    private readonly IDictionary<int, string> _ids = new Dictionary<int, string>();
    // mathces any resource ids in the endpoint that are specified inside a set of curly braces {}
    private const string _resourceIdPattern = @"{(.+?)}";
    // the regex used to get resource ids out of this route's endpoint
    private static readonly Regex _resourceIdRegex = new Regex(_resourceIdPattern, RegexOptions.IgnoreCase);
    // the regex pattern used to get each part of the endpoint.
    private const string _resourceIdMatcher = @"([^/]*)";

    public Route(HttpMethods httpMethod, string endpoint, Action<RoutedHttpRequest, HttpListenerResponse> callback)
    {
      HttpMethod = httpMethod;
      Callback = callback;
      Endpoint = endpoint;
      Regex = BuildRegex(Endpoint);
    }

    /// <summary>
    /// Builds a regex to match resource ids from an endpoint.
    /// </summary>
    /// <param name="endpoint">The endpoint to build the regex from.</param>
    /// <remarks>
    /// Some examples of inputs/outputs to this method are:
    /// /users/ => ^/users$
    /// /users/{userId} => ^/users/([^/]*)$
    /// /users/{userId}/places => ^/users/([^/]*)/places$
    /// /users/{userId}/places/{placeId} => ^/users/([^/]*)/places/([^/]*)$
    /// </remarks>
    /// <returns>The regex used to match endpoints.</returns>
    private Regex BuildRegex(string endpoint)
    {
      var pattern = new StringBuilder();
      // always match endpoints from the start of the string.
      pattern.Append("^");

      int resourceIdIndex = 0;

      // normalize all endpoints to not have leading or trailing forward slashes, and then 
      // split the endpoint into an array of parts
      var parts = endpoint.Trim('/').Split('/');

      foreach (var part in parts)
      {
        // append the endpoint part separator at the beginning of each part.
        pattern.Append("/");

        var match = _resourceIdRegex.Match(part);
        if (match.Success)
        {
          // if this part of the enpoint is an identifier, append the 
          // ([^/]*) regex pattern to pattern, and also store the 
          // name of the param id
          pattern.Append(_resourceIdMatcher);
          _ids[resourceIdIndex++] = match.Groups[1].Value;
        }
        else
        {
          // otherwise just store the current part ofthe endpoint
          pattern.Append(part);
        }
      }

      // always match the endpoints until the end of the string.
      pattern.Append("$");

      return new Regex(pattern.ToString(), RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Gets a dictionary of params from the route request.
    /// </summary>
    /// <param name="match">The successful <see cref="Regex"/> match for this <see cref="Route"/>.</param>
    /// <returns>A dictionary of param ids to param values.</returns>
    public IDictionary<string, object> GetParams(Match match)
    {
      var @params = new Dictionary<string, object>();
      for (int paramId = 0; paramId < _ids.Count; paramId++)
      {
        // the param value offset is always 1 more than the param id
        // because the first item in a Regex.Match.Group is always the entire string.
        @params[_ids[paramId]] = match.Groups[paramId + 1].Value;
      }

      return @params;
    }

    /// <summary>
    /// Gets the type of http method this route responds to.
    /// </summary>
    public HttpMethods HttpMethod { get; private set; }

    /// <summary>
    /// Gets the endpoint for this route, e.g., the endpoint of
    /// http://localhost/api/1?foo=bar would be /api/1
    /// </summary>
    public string Endpoint { get; private set; }

    /// <summary>
    /// Gets the regex used to get resource ids out of the endpoint.
    /// </summary>
    public Regex Regex { get; private set; }

    /// <summary>
    /// Gets the callback that will be raised when an endpoint matches this route.
    /// </summary>
    public Action<RoutedHttpRequest, HttpListenerResponse> Callback { get; private set; }
  }
}
