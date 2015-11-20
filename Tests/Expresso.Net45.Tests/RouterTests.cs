using Xunit;

namespace Expresso
{
  public class RouterTests
  {
    [Fact]
    public void RouterCanRouteSimpleResource()
    {
      var router = new Router();

      bool routerDidRoute = false;

      router.Route(HttpMethods.Get, "/test", (req, res) =>
      {
        routerDidRoute = true;
      });

      // Need to mock httplistenercontext to be able to test for real.
    }
  }
}
