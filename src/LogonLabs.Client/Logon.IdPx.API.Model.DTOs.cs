/* Options:
Date: 2019-05-14 12:24:11
Version: 5.40
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: https://local-idpx.logon-dev.com

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
using LogonLabs.Model;


namespace LogonLabs.Model
{

    [Route("/callback", "GET")]
    [Route("/callback", "POST")]
    [DataContract]
    public partial class Callback
        : IReturn<CallbackResponse>
    {
        [DataMember(IsRequired=true)]
        public virtual string code { get; set; }

        [DataMember(IsRequired=true)]
        public virtual string state { get; set; }

        [DataMember(IsRequired=true)]
        public virtual string session_state { get; set; }

        [DataMember]
        public virtual string RelayState { get; set; }

        [DataMember]
        public virtual string SAMLResponse { get; set; }
    }

    public partial class CallbackResponse
        : IBaseResponse
    {
        public virtual IErrorResponse error { get; set; }
    }

    [Route("/event", "POST")]
    [DataContract]
    public partial class CreateEvent
        : IReturn<CreateEventResponse>, IAppId
    {
        public CreateEvent()
        {
            tags = new Tag[]{};
        }

        [DataMember]
        public virtual string app_id { get; set; }

        [DataMember]
        public virtual string type { get; set; }

        [DataMember]
        public virtual bool validate { get; set; }

        [DataMember]
        public virtual string local_validation { get; set; }

        [DataMember]
        public virtual string email_address { get; set; }

        [DataMember]
        public virtual string first_name { get; set; }

        [DataMember]
        public virtual string last_name { get; set; }

        [DataMember]
        public virtual string ip_address { get; set; }

        [DataMember]
        public virtual string user_agent { get; set; }

        [DataMember]
        public virtual Tag[] tags { get; set; }
    }

    public partial class CreateEventResponse
        : IBaseResponse
    {
        [DataMember]
        public virtual string event_id { get; set; }

        [DataMember]
        public virtual bool? event_success { get; set; }

        [DataMember]
        public virtual ValidationDetails validation_details { get; set; }

        [DataMember]
        public virtual LocationDetails location_details { get; set; }

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
        [DataMember(IsRequired=true)]
        public virtual string app_id { get; set; }

        [DataMember]
        public virtual string email_address { get; set; }
    }

    public partial class GetProvidersResponse
        : IBaseResponse
    {
        public GetProvidersResponse()
        {
            identity_providers = new Provider[]{};
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
        [DataMember(IsRequired=true)]
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
        public StartLogin()
        {
            tags = new Tag[]{};
        }

        [DataMember(IsRequired=true)]
        public virtual string app_id { get; set; }

        [DataMember(IsRequired=true)]
        public virtual string identity_provider { get; set; }

        [DataMember]
        public virtual string email_address { get; set; }

        [DataMember]
        public virtual string client_data { get; set; }

        [DataMember]
        public virtual string client_encryption_key { get; set; }

        [DataMember]
        public virtual Tag[] tags { get; set; }
    }

    public partial class StartLoginResponse
        : IBaseResponse
    {
        public virtual IErrorResponse error { get; set; }
        [DataMember(IsRequired=true)]
        public virtual string token { get; set; }
    }

    [DataContract]
    public partial class Tag
    {
        [DataMember]
        public virtual string key { get; set; }

        [DataMember]
        public virtual string value { get; set; }
    }

    [Route("/event/{event_id}", "PUT")]
    [DataContract]
    public partial class UpdateEvent
        : IReturn<UpdateEventResponse>, IAppId
    {
        public UpdateEvent()
        {
            tags = new Tag[]{};
        }

        [DataMember(IsRequired=true)]
        public virtual string app_id { get; set; }

        [DataMember(IsRequired=true)]
        public virtual string event_id { get; set; }

        [DataMember]
        public virtual string local_success { get; set; }

        [DataMember]
        public virtual Tag[] tags { get; set; }
    }

    public partial class UpdateEventResponse
        : IBaseResponse
    {
        public virtual IErrorResponse error { get; set; }
    }

    [Route("/validate", "POST")]
    [DataContract]
    public partial class ValidateLogin
        : IReturn<ValidateLoginResponse>, IAppId
    {
        [DataMember(IsRequired=true)]
        public virtual string app_id { get; set; }

        [DataMember(IsRequired=true)]
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
        public virtual bool event_success { get; set; }

        [DataMember]
        public virtual ValidationDetails validation_details { get; set; }

        [DataMember]
        public virtual IdentityProviderData identity_provider_data { get; set; }

        [DataMember]
        public virtual string client_data { get; set; }

        [DataMember]
        public virtual string client_encryption_key { get; set; }

        [DataMember]
        public virtual string event_id { get; set; }

        public virtual IErrorResponse error { get; set; }
    }

    public partial class ValidationDetails
    {
        [DataMember]
        public virtual string auth_validation { get; set; }

        [DataMember]
        public virtual string email_match_validation { get; set; }

        [DataMember]
        public virtual string ip_validation { get; set; }

        [DataMember]
        public virtual string geo_validation { get; set; }

        [DataMember]
        public virtual string time_validation { get; set; }
    }
}
