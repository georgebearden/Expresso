# Expresso

Inspiration for the Expresso library came from the [Express](http://expressjs.com/guide/routing.html) routing code.  It greatly simplifies creating a RESTful web-service in C#.  A simple example of using Expresso that maps the _/users_ endpoint of a url to a callback is

```csharp
var router = new Router();
router.Route(HttpMethods.Get, "/users", (req, res) => 
{

});
```

The HttpRouter also provides a way to get parameters from an endpoint.  An example that maps the _/users/georgebearden_ endpoint of a url to callback is

```csharp
var router = new Router();
router.Route(HttpMethods.Get, "/users/{userId}", (req, res) => 
{
  var userId = req.Params["userId"];
});
```

### Platforms supported
* Windows Desktop .NET 4.5
* Xamarin support coming soon...

### Next steps
Adding support for middleware.
