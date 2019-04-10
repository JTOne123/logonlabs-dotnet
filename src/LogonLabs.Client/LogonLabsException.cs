using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;

namespace LogonLabs.IdPx.API
{
    public class LogonLabsException : Exception
    {
        public HttpStatusCode http_status_code { get; set; }
        public string error_code { get; set; }
        public string error_message { get; set; }


        internal LogonLabsException(HttpStatusCode http_status_code, string error_code, string error_message, Exception inner_exception) : base(error_message, inner_exception)
        {
            this.http_status_code = http_status_code;
            this.error_code = error_code;
            this.error_message = error_message;


        }

        internal static LogonLabsException GetException(WebServiceException ex)
        {
            var genericBaseResponse = ServiceStack.Text.JsonSerializer.DeserializeFromString<System.Dynamic.ExpandoObject>(ex.ResponseBody);


            string error_code = null;
            string error_message = null;

            var baseResponseDict = genericBaseResponse as IDictionary<string, object>;
            if (baseResponseDict.ContainsKey("error"))
            {
                var errorDict = baseResponseDict["error"] as IDictionary<string, object>;

                if (errorDict.ContainsKey("code"))
                {
                    error_code = (string)errorDict["code"];
                }

                if (errorDict.ContainsKey("message"))
                {
                    error_message = (string)errorDict["message"];
                }
            }


            return new LogonLabsException((HttpStatusCode)ex.StatusCode, error_code, error_message, ex);
        }

    }
}
