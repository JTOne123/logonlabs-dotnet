
using LogonLabs.IdPx.API.Model;

using ServiceStack;
using System;
using System.IO;
using System.Security.Cryptography;


namespace LogonLabs
{
    public class LogonClient : ILogonClient
    {
        private string _baseUrl;

        private string _appId;

        private string _appSecret;

        private ServiceStack.JsonHttpClient _client;

        public LogonClient(string appId, string baseUrl, string appSecret = null)
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new LogonLabsException(System.Net.HttpStatusCode.Unused, Constants.ErrorCodes.ApiError, "appId cannot be empty", null);
            }

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new LogonLabsException(System.Net.HttpStatusCode.Unused, Constants.ErrorCodes.ApiError, "baseUrl cannot be empty", null);
            }

            _baseUrl = baseUrl;
            _appId = appId;
            _appSecret = appSecret;
            _client = new ServiceStack.JsonHttpClient(_baseUrl);

            if (!string.IsNullOrWhiteSpace(appSecret))
            {
                _client.AddHeader(Constants.Headers.AppSecret, appSecret);
            }
        }

        private void ThrowIfNoSecret()
        {
            if (string.IsNullOrWhiteSpace(_appSecret))
            {
                throw new LogonLabsException(System.Net.HttpStatusCode.Unused, Constants.ErrorCodes.ApiError, "appSecret cannot be empty", null);
            }
        }
        public string Ping()
        {
            try
            {
                var resp = _client.Get(new Ping() { app_id = _appId });
                return resp?.version;
            }
            catch (WebServiceException ex)
            {
                throw LogonLabsException.GetException(ex);
            }

        }

        public GetProvidersResponse GetProviders(string emailAddress = null)
        {
            try
            {
                var resp = _client.Get(new GetProviders() { app_id = _appId, email_address = emailAddress });

                return resp;
            }
            catch (WebServiceException ex)
            {
                throw LogonLabsException.GetException(ex);
            }
        }
        public string StartLogin(string identityProvider = null, string identityProviderId = null, string emailAddress = null, string clientData = null, string callbackUrl = null, string destinationUrl = null, Tag[] tags = null)
        {
            try
            {
                var ssoStartResponse = _client.Post(new StartLogin() { app_id = _appId, identity_provider = identityProvider, identity_provider_id = identityProviderId, email_address = emailAddress, client_data = clientData, callback_url = callbackUrl, destination_url = destinationUrl, tags = tags });

                return $"{_client.BaseUri}{new RedirectLogin() { token = ssoStartResponse.token }.ToGetUrl()}";
            }
            catch (WebServiceException ex)
            {
                throw LogonLabsException.GetException(ex);
            }
        }



        public ValidateLoginResponse ValidateLogin(string token)
        {
            ThrowIfNoSecret();

            try
            {
                var resp = _client.Post(new ValidateLogin() { app_id = _appId, token = token });
                return resp;
            }
            catch (WebServiceException ex)
            {
                throw LogonLabsException.GetException(ex);
            }
        }

        public UpdateEventResponse UpdateEvent(string eventId, string localValidation = null, Tag[] tags = null)
        {
            ThrowIfNoSecret();

            try
            {
                var resp = _client.Put(new UpdateEvent() { app_id = _appId, event_id = eventId, local_validation = localValidation, tags = tags });
                return resp;
            }
            catch (WebServiceException ex)
            {
                throw LogonLabsException.GetException(ex);
            }
        }
        public CreateEventResponse CreateEvent(string type, bool validate, string localValidation, string emailAddress, string ipAddress, string userAgent = null, string firstName = null, string lastName = null, Tag[] tags = null)
        {
            ThrowIfNoSecret();

            try
            {
                var resp = _client.Post(new CreateEvent() { app_id = _appId, type = type, validate = validate, local_validation = localValidation, email_address = emailAddress, ip_address = ipAddress, user_agent = userAgent, first_name = firstName, last_name = lastName, tags = tags });
                return resp;
            }
            catch (WebServiceException ex)
            {
                throw LogonLabsException.GetException(ex);
            }
        }

    }


}
