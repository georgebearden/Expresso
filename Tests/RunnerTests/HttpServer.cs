using HttpRouter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RunnerTests
{
  public class HttpServer : IDisposable
  {
    private readonly HttpListener _server;
    private readonly Task _serverTask;
    private readonly CancellationTokenSource _cancelTokenSource;

    public HttpServer(Router router)
    {
      _cancelTokenSource = new CancellationTokenSource();

      _server = new HttpListener();
      _server.Prefixes.Add(@"http://localhost:5556/");
      _server.Start();

      _serverTask = Task.Run(async () =>
      {
        while (!_cancelTokenSource.IsCancellationRequested)
        {
          var context = await Task.Run(() => _server.GetContext(), _cancelTokenSource.Token);

          var endpoint = context.Request.Url.LocalPath;
          router.HandleRequest(endpoint, context);
        }
      }, _cancelTokenSource.Token);
    }

    public void Dispose()
    {
      if (_cancelTokenSource.IsCancellationRequested)
        return;

      _cancelTokenSource.Cancel();
    }
  }
}
