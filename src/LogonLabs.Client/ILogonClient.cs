using LogonLabs.IdPx.API.Model;

namespace LogonLabs.IdPx.API
{
    public interface ILogonClient
    {

        string StartLogin(string identity_provider, string email_address = null, string client_data = null, string client_encryption_key = null);


        ValidateLoginResponse ValidateLogin(string token);



        GetProvidersResponse GetProviders(string email_address = null);
        string Ping();

        string Encrypt(string client_encryption_key, string value);
        string Decrypt(string client_encryption_key, string value);
    }
}