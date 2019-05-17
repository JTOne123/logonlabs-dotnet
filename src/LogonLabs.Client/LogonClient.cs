
using LogonLabs.Model;
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
                throw new LogonLabsException(System.Net.HttpStatusCode.Unused, Constants.ErrorCodes.api_error, "appId cannot be empty", null);
            }

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new LogonLabsException(System.Net.HttpStatusCode.Unused, Constants.ErrorCodes.api_error, "baseUrl cannot be empty", null);
            }

            _baseUrl = baseUrl;
            _appId = appId;
            _appSecret = appSecret;
            _client = new ServiceStack.JsonHttpClient(_baseUrl);

            if (!string.IsNullOrWhiteSpace(appSecret))
            {
                _client.AddHeader(Constants.Headers.app_secret, appSecret);
            }
        }

        private void ThrowIfNoSecret()
        {
            if (string.IsNullOrWhiteSpace(_appSecret))
            {
                throw new LogonLabsException(System.Net.HttpStatusCode.Unused, Constants.ErrorCodes.api_error, "appSecret cannot be empty", null);
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
        public string StartLogin(string identityProvider, string emailAddress = null, string clientData = null,
            string clientEncryptionKey = null, Tag[] tags = null)
        {
            try
            {
                var ssoStartResponse = _client.Post(new StartLogin() { app_id = _appId, identity_provider = identityProvider, email_address = emailAddress, client_data = clientData, client_encryption_key = clientEncryptionKey, tags = tags });

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

        public UpdateEventResponse UpdateEvent(string eventId, string localSuccess = null, Tag[] tags = null)
        {
            ThrowIfNoSecret();

            try
            {
                var resp = _client.Put(new UpdateEvent() { app_id = _appId, event_id = eventId, local_success = localSuccess, tags = tags });
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

        public string Encrypt(string clientEncryptionKey, string value)
        {
            byte[] encrypted;
            byte[] IV;
            byte[] Salt = GetSalt();

            byte[] Key = CreateKey(clientEncryptionKey, Salt);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;

                aesAlg.GenerateIV();
                IV = aesAlg.IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(value);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            byte[] combinedIvSaltCt = new byte[Salt.Length + IV.Length + encrypted.Length];
            Array.Copy(Salt, 0, combinedIvSaltCt, 0, Salt.Length);
            Array.Copy(IV, 0, combinedIvSaltCt, Salt.Length, IV.Length);
            Array.Copy(encrypted, 0, combinedIvSaltCt, Salt.Length + IV.Length, encrypted.Length);

            return Convert.ToBase64String(combinedIvSaltCt.ToArray());
        }

        public string Decrypt(string clientEncryptionKey, string value)
        {
            byte[] inputAsByteArray;
            string plaintext = null;

            inputAsByteArray = Convert.FromBase64String(value);

            byte[] Salt = new byte[32];
            byte[] IV = new byte[16];
            byte[] Encoded = new byte[inputAsByteArray.Length - Salt.Length - IV.Length];

            Array.Copy(inputAsByteArray, 0, Salt, 0, Salt.Length);
            Array.Copy(inputAsByteArray, Salt.Length, IV, 0, IV.Length);
            Array.Copy(inputAsByteArray, Salt.Length + IV.Length, Encoded, 0, Encoded.Length);

            byte[] Key = CreateKey(clientEncryptionKey, Salt);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Encoded))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;

        }


        #region Encryption 

        private static readonly int iterations = 1000;
        private static byte[] CreateKey(string password, byte[] salt)
        {
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
                return rfc2898DeriveBytes.GetBytes(32);
        }

        private static byte[] GetSalt()
        {
            var salt = new byte[32];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        #endregion
    }


}
