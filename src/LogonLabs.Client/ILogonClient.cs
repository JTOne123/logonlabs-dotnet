using LogonLabs.IdPx.API.Model;


namespace LogonLabs
{
    public interface ILogonClient
    {

        string StartLogin(string identityProvider = null, string identityProviderId = null, string emailAddress = null, string clientData = null, string callbackUrl = null, string destinationUrl = null, Tag[] tags = null);


        ValidateLoginResponse ValidateLogin(string token);

        UpdateEventResponse UpdateEvent(string eventId, string localSuccess = null, Tag[] tags = null);

        CreateEventResponse CreateEvent(string type, bool validate, string localValidation, string emailAddress,
            string ipAddress, string userAgent = null, string firstName = null, string lastName = null,
            Tag[] tags = null);
        GetProvidersResponse GetProviders(string emailAddress = null);
        string Ping();

    }
}