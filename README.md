The official LogonLabs .NET API library.

## Prerequisites
<.NET version support?>

## Documentation

For the full Developer Documentation please visit: https://logonlabs.com/docs/api/

### Install LogonLabs (NuGet)

Via command line:

	nuget install LogonLabs.Client

Via Package Manager:

	PM> Install-Package LogonLabs.Client

Via Visual Studio:

1. Open the Solution Explorer.
2. Right-click on a project within your solution.
3. Click on *Manage NuGet Packages...*
4. Click on the *Browse* tab and search for "LogonLabs".
5. Click on the LogonLabs.Client package, select the appropriate version in the right-tab and click *Install*.


## Usage

### Client Side Workflow

Use the snippet below to initiate the SSO login process for you user. This will start the Login process by starting a redirect session and redirect to the LogonLabs to broker the SSO request with the desired identity provider.

```csharp
var logonClient = new LogonLabs.IdPx.API.LogonClient("YOUR_APP_ID");
var redirectUrl = _logonClient.StartLogin(IdentityProviderEnum.google);
Response.Redirect(redirectUrl);
```

### Server Side Workflows

Use the snippet below to validate the login data from LogonLabs and continue your app's authentication workflows (ie, create a user session token).

```csharp
var logonClient = new Logon.IdPx.API.LogonClient("YOUR_APP_ID", "YOUR_APP_SECRET");

//NOTE: depending on what flavor of .NET you are using (Asp.Net Core, .NET Framework), this could be slightly different
var token = Request.Query["token"];

var loginData = _logonClient.ValidateLogin(token);
if (!loginData.validation_success)
{
    throw new Exception("Failed to validate the user login request.");
}

//Success! Continue your app's login workflow and create a user session, etc!
```