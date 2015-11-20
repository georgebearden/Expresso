using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpRouter
{
  /// <summary>
  /// Composes a <see cref="HttpListenerRequest"/> with a set of url path key/value pairs.
  /// </summary>
  public class RoutedHttpRequest
  {
    /// <summary>
    /// Creates a new instance of the <see cref="RoutedHttpRequest"/> class.
    /// </summary>
    /// <param name="request">The unmodified <see cref="HttpListenerRequest"/> from the <see cref="HttpListenerContext"/>.</param>
    /// <param name="params">The mapping of key/value pairs that correspond to the resource ids in the url path.</param>
    public RoutedHttpRequest(HttpListenerRequest request, IDictionary<string, object> @params)
    {
      Request = request;
      Params = @params;
    }

    /// <summary>
    /// Gets the <see cref="HttpListenerRequest"/>.
    /// </summary>
    public HttpListenerRequest Request { get; private set; }

    /// <summary>
    /// Gets the resource id key value pairs.
    /// </summary>
    public IDictionary<string, object> Params { get; private set; }
  }
}
