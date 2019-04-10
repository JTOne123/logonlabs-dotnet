/* Options:
Date: 2019-04-09 23:05:09
Version: 5.40
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: https://api.logon-dev.com

//GlobalNamespace: 
//MakePartial: True
//MakeVirtual: True
//MakeInternal: False
//MakeDataContractsExtensible: False
//AddReturnMarker: True
//AddDescriptionAsComments: True
//AddDataContractAttributes: False
//AddIndexesToDataMembers: False
//AddGeneratedCodeAttributes: False
//AddResponseStatus: False
//AddImplicitVersion: 
//InitializeCollections: True
//ExportValueTypes: False
//IncludeTypes: 
//ExcludeTypes: 
//AddNamespaces: 
//AddDefaultXmlNamespace: http://schemas.servicestack.net/types
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.DataAnnotations;
using LogonLabs.IdPx.API.Model;


namespace LogonLabs.IdPx.API.Model
{

    [Route("/callback", "GET")]
    [DataContract]
    public partial class Callback
        : IReturn<CallbackResponse>
    {
        [DataMember(IsRequired = true)]
        public virtual string code { get; set; }

        [DataMember(IsRequired = true)]
        public virtual string state { get; set; }

        [DataMember(IsRequired = true)]
        public virtual string session_state { get; set; }
    }

    public partial class CallbackResponse
        : IBaseResponse
    {
        public virtual IErrorResponse error { get; set; }
    }

    [Route("/error")]
    [DataContract]
    public partial class ErrorPage
        : IReturn<Object>
    {
    }

    [Route("/providers", "GET")]
    [DataContract]
    public partial class GetProviders
        : IReturn<GetProvidersResponse>, IAppId
    {
        [DataMember(IsRequired = true)]
        public virtual string app_id { get; set; }

        [DataMember]
        public virtual string email_address { get; set; }
    }

    public partial class GetProvidersResponse
        : IBaseResponse
    {
        public GetProvidersResponse()
        {
            identity_providers = new Provider[] { };
        }

        [DataMember]
        public virtual Provider[] identity_providers { get; set; }

        [DataMember]
        public virtual string suggested_identity_provider { get; set; }

        public virtual IErrorResponse error { get; set; }
    }

    public partial interface IAppId
    {
        string app_id { get; set; }
    }

    public partial interface IBaseResponse
    {
        IErrorResponse error { get; set; }
    }

    public partial class IdentityProviderData
    {
        [DataMember]
        public virtual string identity_provider_type { get; set; }

        [DataMember]
        public virtual string uid { get; set; }

        [DataMember]
        public virtual string first_name { get; set; }

        [DataMember]
        public virtual string last_name { get; set; }
    }

    public partial interface IErrorResponse
    {
        string code { get; set; }
        string message { get; set; }
    }

    public partial class LocationDetails
    {
        [DataMember]
        public virtual string continent_code { get; set; }

        [DataMember]
        public virtual string continent_name { get; set; }

        [DataMember]
        public virtual string country_code { get; set; }

        [DataMember]
        public virtual string country_name { get; set; }
    }

    [Route("/ping", "GET")]
    [DataContract]
    public partial class Ping
        : IReturn<PingResponse>, IAppId
    {
        [DataMember]
        public virtual string app_id { get; set; }
    }

    public partial class PingResponse
        : IBaseResponse
    {
        [DataMember]
        public virtual string version { get; set; }

        public virtual IErrorResponse error { get; set; }
    }

    public partial class Provider
    {
        [DataMember]
        public virtual string type { get; set; }
    }

    [Route("/redirect", "GET")]
    [DataContract]
    public partial class RedirectLogin
        : IReturn<RedirectLoginResponse>
    {
        [DataMember(IsRequired = true)]
        public virtual string token { get; set; }
    }

    public partial class RedirectLoginResponse
        : IBaseResponse
    {
        public virtual IErrorResponse error { get; set; }
    }

    [Route("/start", "POST")]
    [DataContract]
    public partial class StartLogin
        : IReturn<StartLoginResponse>, IAppId
    {
        [DataMember(IsRequired = true)]
        public virtual string app_id { get; set; }

        [DataMember(IsRequired = true)]
        public virtual string identity_provider { get; set; }

        [DataMember]
        public virtual string email_address { get; set; }

        [DataMember]
        public virtual string client_data { get; set; }

        [DataMember]
        public virtual string client_encryption_key { get; set; }
    }

    public partial class StartLoginResponse
        : IBaseResponse
    {
        public virtual IErrorResponse error { get; set; }
        [DataMember(IsRequired = true)]
        public virtual string token { get; set; }
    }

    [Route("/validate", "POST")]
    [DataContract]
    public partial class ValidateLogin
        : IReturn<ValidateLoginResponse>, IAppId
    {
        [DataMember(IsRequired = true)]
        public virtual string app_id { get; set; }

        [DataMember(IsRequired = true)]
        public virtual string token { get; set; }
    }

    public partial class ValidateLoginResponse
        : IBaseResponse
    {
        [DataMember]
        public virtual string app_id { get; set; }

        [DataMember]
        public virtual string email_address { get; set; }

        [DataMember]
        public virtual string ip_address { get; set; }

        [DataMember]
        public virtual LocationDetails location { get; set; }

        [DataMember]
        public virtual bool validation_success { get; set; }

        [DataMember]
        public virtual ValidateLoginResponse.SsoValidationDetails validation_details { get; set; }

        [DataMember]
        public virtual IdentityProviderData identity_provider_data { get; set; }

        [DataMember]
        public virtual string client_data { get; set; }

        [DataMember]
        public virtual string client_encryption_key { get; set; }

        public virtual IErrorResponse error { get; set; }

        public partial class SsoValidationDetails
        {
            [DataMember]
            public virtual bool auth_valid { get; set; }

            [DataMember]
            public virtual bool email_match_valid { get; set; }

            [DataMember]
            public virtual bool ip_valid { get; set; }

            [DataMember]
            public virtual bool geo_valid { get; set; }

            [DataMember]
            public virtual bool time_valid { get; set; }
        }
    }
}
