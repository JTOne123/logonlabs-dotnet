using System;
using System.Collections.Generic;
using System.Text;

namespace LogonLabs.IdPx.API
{
    public static class Constants
    {

        public static class ErrorCodes
        {
            public const string api_error = "api_error";
            public const string validation_error = "validation_error";


        }
        public static class IdentityProviders
        {
            public const string Google = "google";
            public const string Microsoft = "microsoft";
            public const string Facebook = "facebook";
            public const string LinkedIn = "linkedin";
            public const string Okta = "okta";

        }

        public static class Headers
        {
            public const string app_secret = "x-app-secret";
        }

        public static class QueryString
        {
            public const string token = "token";
        }

    }
}
