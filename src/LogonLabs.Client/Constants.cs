using System;
using System.Collections.Generic;
using System.Text;

namespace LogonLabs
{
    public static class Constants
    {

        public static class ErrorCodes
        {
            public const string ApiError = "api_error";
            public const string ValidationError = "validation_error";
            public const string ForbiddenError = "forbidden_error";
            public const string UnauthorizedError = "unauthorized_error";


        }
        public static class IdentityProviders
        {
            public const string Google = "google";
            public const string Microsoft = "microsoft";
            public const string Facebook = "facebook";
            public const string LinkedIn = "linkedin";
            public const string Okta = "okta";
			public const string Slack = "slack";
			public const string Quickbooks = "quickbooks";
			public const string Github = "github";
            public const string OneLogin = "onelogin";
        }

        public static class EventTypes
        {
            public const string LocalLogin = "LocalLogin";
            public const string LocalLogout = "LocalLogout";
        }

        public static class Headers
        {
            public const string AppSecret = "x-app-secret";
        }

        public static class QueryString
        {
            public const string Token = "token";
            public const string AppId = "app_id";

        }

        public static class EventValidationTypes
        {
            public const string Pass = "Pass";
            public const string Fail = "Fail";
            public const string NotApplicable = "NotApplicable";
        }

    }
}
