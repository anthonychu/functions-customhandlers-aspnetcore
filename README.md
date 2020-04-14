# ASP.NET Core in Azure Functions Custom Handlers

> Important: This is a quick proof of concept and how this works is subject to change until [custom handlers](https://docs.microsoft.com/en-us/azure/azure-functions/functions-custom-handlers) reaches general availability.

## dotnet-react (.NET Core + React app)

This is an ASP.NET Core app built with `dotnet new react`. There were 3 main changes:

* `Program.cs`
    - Listen to `FUNCTIONS_HTTPWORKER_PORT`.
    - Change current working directory to `server` as the app will run from a subfolder within a function app. This is configured using an app setting named `ServerSubfolder`.
* `Startup.cs`
    - Add middleware to rewrite the request path using the `__path__` query string.

### Build and publish the app

Publish the app into a folder named `server` in the function app.

```bash
cd dotnet-react
dotnet publish -c Release -o ../function-app/server
```

## function-app (main Azure Functions app)

This is the main function app. `host.json` has been configured to start `dotnet server/dotnet-react.dll` as the custom hander.

A proxy is configured in `proxies.json` to forward all requests to `/api/index?__path__={path}` in the function app. This is currently needed as the original path required for routing in ASP.NET Core is lost when the custom handler is invoked.

There's an `index` function that invokes the custom handler.

### Run locally

```bash
func start
```

### Publish to Azure

Create a function app in the Windows consumption plan, then publish with this command:

```bash
func azure functionapp publish $FUNCTION_APP_NAME --no-build -b local --force
```

