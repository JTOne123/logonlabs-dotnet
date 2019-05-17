using LogonLabs.Model;

namespace LogonLabs
{
    public interface ILogonClient
    {

        string StartLogin(string identityProvider, string emailAddress = null, string clientData = null,
            string clientEncryptionKey = null, Tag[] tags = null);


        ValidateLoginResponse ValidateLogin(string token);

        UpdateEventResponse UpdateEvent(string eventId, string localSuccess = null, Tag[] tags = null);

        CreateEventResponse CreateEvent(string type, bool validate, string localValidation, string emailAddress,
            string ipAddress, string userAgent = null, string firstName = null, string lastName = null,
            Tag[] tags = null);
        GetProvidersResponse GetProviders(string emailAddress = null);
        string Ping();

        string Encrypt(string clientEncryptionKey, string value);
        string Decrypt(string clientEncryptionKey, string value);
    }
}