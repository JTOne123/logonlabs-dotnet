
using LogonLabs.IdPx.API.Model;
using ServiceStack;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;


namespace LogonLabs.IdPx.API
{
    public class LogonClient : ILogonClient
    {
        private string _idpx_url;

        private string _app_id;

        private string _app_secret;

        private ServiceStack.JsonHttpClient _client;

        public LogonClient(string app_id, string app_secret = null, string idpx_url = "https://api.logonlabs.com")
        {
            if (string.IsNullOrWhiteSpace(app_id))
            {
                throw new Exception("app_id cannot be empty");
            }

            _idpx_url = idpx_url;
            _app_id = app_id;
            _app_secret = app_secret;
            _client = new ServiceStack.JsonHttpClient(_idpx_url);

            if (!string.IsNullOrWhiteSpace(app_secret))
            {
                _client.AddHeader("x-app-secret", app_secret);
            }
        }

        private void ThrowIfNoSecret()
        {
            if (string.IsNullOrWhiteSpace(_app_secret))
            {
                throw new Exception("app_secret cannot be empty");
            }
        }
        public string Ping()
        {
            var resp = _client.Get(new Ping() { app_id = _app_id });

            return resp?.version;
        }

        public GetProvidersResponse GetProviders(string email_address = null)
        {
            var resp = _client.Get(new GetProviders() { app_id = _app_id, email_address = email_address });

            return resp;
        }
        public string StartLogin(string identity_provider, string email_address = null, string client_data = null, string client_encryption_key = null)
        {
            var ssoStartResponse = _client.Post(new StartLogin() { app_id = _app_id, identity_provider = identity_provider, email_address = email_address, client_data = client_data, client_encryption_key = client_encryption_key });

            return $"{_client.BaseUri}{new RedirectLogin() { token = ssoStartResponse.token }.ToGetUrl()}";
        }



        public ValidateLoginResponse ValidateLogin(string token)
        {
            ThrowIfNoSecret();
            var resp = _client.Post(new ValidateLogin() { app_id = _app_id, token = token });
            return resp;
        }

        public ValidateLocalLoginResponse ValidateLocalLogin(string email_address, string ip_address = null, string user_agent = null, string client_data = null)
        {
            ThrowIfNoSecret();
            var resp = _client.Post(new ValidateLocalLogin() { app_id = _app_id, email_address = email_address, ip_address = ip_address, user_agent = user_agent, client_data = client_data });
            return resp;
        }

        public string Encrypt(string client_encryption_key, string value)
        {
            byte[] encrypted;
            byte[] IV;
            byte[] Salt = GetSalt();

            byte[] Key = CreateKey(client_encryption_key, Salt);

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

        public string Decrypt(string client_encryption_key, string value)
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

            byte[] Key = CreateKey(client_encryption_key, Salt);

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
