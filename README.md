

# LogonLabs .Net
---
The official LogonLabs .NET API library.

### Install LogonLabs (NuGet)
---
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

## Logon Labs API
---

- Prior to coding, some configuration is required at https://app.logonlabs.com/app/#app-settings.

- For the full Developer Documentation please visit: https://app.logonlabs.com/api/

---
### Instantiating a new client

- `APP_ID` can be found in [App Settings](https://app.logonlabs.com/app/#/app-settings)
- `APP_SECRET` is configured at [App Secrets](https://app.logonlabs.com/app/#/app-secrets)
- `LOGONLABS_API_ENDPOINT` should be set to `https://api.logonlabs.com`

Create a new instance of `LogonClient`.  

```csharp
using LogonLabs;

LogonClient client = new LogonClient("{APP_ID}", baseUrl: "{LOGONLABS_API_ENDPOINT}", appSecret: "{APP_SECRET}");
```
---
### SSO Login QuickStart

The StartLogin function in the JS library begins the Logon Labs managed SSO process.  

>Further documentation on starting the login process via our JavaScript client can be found at our GitHub page [here](https://github.com/logonlabs/logonlabs-js)

The following example demonstrates what to do once the `Callback Url` has been used by our system to redirect the user back to your page:

```csharp
using LogonLabs;

//NOTE: depending on what flavor of .NET you are using (Asp.Net Core, .NET Framework), this could be slightly different
var token = Request.Query[Constants.QueryString.token];

ValidateLoginResponse response = client.validateLogin(token);

if(response.event_success) 
{
    //authentication and validation succeeded. proceed with post-auth workflows (ie, create a user session token for your system).
}

```
---
### C# Only Workflow
The following workflow is required if you're using C# to handle both the front and back ends.  If this does not apply to you please refer to the SSO Login QuickStart section.
#### Step 1 - StartLogin
This call begins the Logon Labs managed SSO process.  The `clientData` property is optional and is used to pass any data that is required after validating the request.  The `tags`property is an ArrayList of type Tag which is a simple object representing a key/value pair.

```csharp
using LogonLabs;
using LogonLabs.Model;
using System.Collections.Generic;

//optional parameters
var clientData = "{\"ClientData\":\"Value\"}";
var tags = new List<Tag>();
var tag = new Tag();
tag.key = "example-key";
tag.value = "example-value";
tags.Add(tag);
//

var redirectUri = client.startLogin(IdentityProviders.Google, null, "example@emailaddress.com", clientData, null, null, tags);
```
The `redirectUri` property returned should be redirected to by the application.  Upon the user completing entering their credentials they will be redirected to the `CallbackUrl` set within the application settings at https://app.logonlabs.com/app/#/app-settings.

#### Step 2 - ValidateLogin
This method is used to validate the results of the login attempt.  `queryToken` corresponds to the query parameter with the name `token` appended to the callback url specified for your app.

The response contains all details of the login and the user has now completed the SSO workflow.  If there is any additional information to add UpdateEvent can be called on the `eventId` returned.
```csharp
using LogonLabs;

//NOTE: depending on what flavor of .NET you are using (Asp.Net Core, .NET Framework), the code to extract the header value could be slightly different
var token = Request.Query[Constants.QueryString.token];

ValidateLoginResponse response = client.ValidateLogin(token);

if(response.event_success) 
{
    //authentication and validation succeeded. proceed with post-auth workflows (ie, create a user session token for your system).
}
else 
{
    //some validations failed.  details contained in SsoValidationDetails object.
    ValidationDetails validationDetails = response.validation_details;
	
    if(string.Equals(validationDetails.domain_validation, Constants.EventValidationTypes.Fail)) 
    {
        //provider used was not enabled for the domain of the user that was authenticated
    }
	
    if(string.Equals(validationDetails.geo_validation, Constants.EventValidationTypes.Fail) 
        || string.Equals(validationDetails.ip_validation, Constants.EventValidationTypes.Fail)
        || string.Equals(validationDetails.time_validation, Constants.EventValidationTypes.Fail)) 
    {
        //validation failed via restriction settings for the app
    }

}

```
---
### CreateEvent
The CreateEvent method allows one to create events that are outside of our SSO workflows.

```csharp
using LogonLabs;
using LogonLabs.Model;
using System.Collections.Generic;

var validateEvent = true;
var tags = new List<Tag>();
var tag = new Tag();
tag.key = "example-key";
tag.value = "example-value";
tags.Add(tag);
var localValidation = Constants.EventValidationTypes.Pass;

CreateEventResponse response = client.CreateEvent(
    Constants.EventTypes.LocalLogin,
    validateEvent,
    localValidation,
    "{IP_ADDRESS}",
    "{EMAIL_ADDRESS}",
    "{FIRST_NAME}",
    "{LAST_NAME}",
    "{USER_AGENT}",
    tags);

```

---
### Helper Methods
#### GetProviders
This method is used to retrieve a list of all providers enabled for the application.
If an email address is passed it will further filter any providers available/disabled for the domain of the address.  
If any Enterprise Identity Providers have been configured a separate set of matching providers will also be returned in enterprise_identity_providers.
```csharp
using LogonLabs;

GetProvidersResponse response = client.GetProviders("example@emailaddress.com");
var suggestedProvider = response.suggested_identity_provider; //use suggested provider in UI
foreach(var provider in response.social_identity_providers) 
{
    //each individual provider available for this app / email address
}

foreach(var enterpriseProvider in response.enterprise_identity_providers)
{
    //each enterprise provider available for this app / email address
}
```