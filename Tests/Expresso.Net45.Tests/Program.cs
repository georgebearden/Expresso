using System;
using System.Net;
using Expresso;

namespace Expresso
{
  public class Program
  {
    public static void Callback(RoutedHttpRequest request, HttpListenerResponse response)
    {
      Console.WriteLine(request.Request.Url);
      foreach (var param in request.Params)
      {
        Console.WriteLine("{0}={1}", param.Key, param.Value);
      }
      response.StatusCode = 200;
      response.OutputStream.Close();
    }

    public static void Main(string[] args)
    {
      var router = new Router();
      router.Route(HttpMethods.Get, "/entities", Callback);
      router.Route(HttpMethods.Get, "/entities/", Callback);
      router.Route(HttpMethods.Get, "/entities/{userId}", Callback);
      router.Route(HttpMethods.Get, "/entities/{userId}/", Callback);
      router.Route(HttpMethods.Get, "/entities/{userId}/names", Callback);
      router.Route(HttpMethods.Get, "/entities/{userId}/names/", Callback);
      router.Route(HttpMethods.Get, "/entities/{userId}/names/{nameId}", Callback);
      router.Route(HttpMethods.Get, "/entities/{userId}/names/{nameId}/", Callback);

      using (var httpServer = new HttpServer(router))
      {
        Console.ReadLine();
      }
    }
  }
}
