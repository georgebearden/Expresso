using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;

namespace Expresso
{
  public class Router
  {
    private readonly HashSet<Route> _routes = new HashSet<Route>();

    public void HandleRequest(string request, HttpListenerContext httpContext)
    {
      // normalize all requests to not have a trailing slash
      request = request.TrimEnd('/');

      // filter the routes to try by their http method type.
      var routes = _routes.Where(route => string.Equals(route.HttpMethod.ToString(), httpContext.Request.HttpMethod, StringComparison.InvariantCultureIgnoreCase));
      foreach (var route in routes)
      {
        var match = route.Regex.Match(request);
        if (match.Success)
        {
          // get the params from the endpoint for this route.
          var @params = route.GetParams(match);
          // compose the HttpListenerRequest with the endpoint's params.
          var routedRequest = new RoutedHttpRequest(httpContext.Request, @params);

          // raise the callback and return.
          route.Callback(routedRequest, httpContext.Response);
          return;
        }
      }
    }

    /// <summary>
    /// Creates a new route that will respond to Http requests.
    /// </summary>
    /// <param name="httpMethod">The type of http method to respond to.</param>
    /// <param name="endpoint">The endpoint for the route to match.</param>
    /// <param name="callback">The callback to raise when an incoming http request matches the <paramref name="httpMethod"/>
    /// and <paramref name="endpoint"/>.</param>
    public void Route(HttpMethods httpMethod, string endpoint, Action<RoutedHttpRequest, HttpListenerResponse> callback)
    {
      _routes.Add(new Route(httpMethod, endpoint, callback));
    }
  }
}
