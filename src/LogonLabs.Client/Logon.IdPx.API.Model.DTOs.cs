/* Options:
Date: 2019-10-10 21:32:46
Version: 5.60
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: https://api.logonlabs.com

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
    [Route("/callback", "POST")]
    [DataContract]
    public partial class Callback
        : IReturn<string>
    {
        [DataMember]
        public virtual string code { get; set; }

        [DataMember]
        public virtual string state { get; set; }

        [DataMember]
        public virtual string session_state { get; set; }

        [DataMember]
        public virtual string RelayState { get; set; }

        [DataMember]
        public virtual string SAMLResponse { get; set; }

        [DataMember]
        public virtual string oauth_token { get; set; }

        [DataMember]
        public virtual string oauth_verifier { get; set; }
    }

    [Route("/entities", "DELETE")]
    [DataContract]
    public partial class ClearEntities
        : IReturn<ClearEntitiesResponse>
    {
        public ClearEntities()
        {
            app_ids = new List<Guid> { };
        }

        [DataMember]
        public virtual List<Guid> app_ids { get; set; }

        [DataMember]
        public virtual bool? clear_social { get; set; }
    }

    [DataContract]
    public partial class ClearEntitiesResponse
        : IBaseResponse
    {
        [DataMember]
        public virtual IErrorResponse error { get; set; }
    }

    [Route("/events", "POST")]
    [DataContract]
    public partial class CreateEvent
        : IReturn<CreateEventResponse>, IAppId
    {
        public CreateEvent()
        {
            tags = new Tag[] { };
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

    public partial class EnterpriseProvider
    {
        [DataMember]
        public virtual string name { get; set; }

        [DataMember]
        public virtual string identity_provider_id { get; set; }

        [DataMember]
        public virtual string type { get; set; }

        [DataMember]
        public virtual string login_button_image_uri { get; set; }

        [DataMember]
        public virtual string login_background_hex_color { get; set; }

        [DataMember]
        public virtual string login_icon_image_uri { get; set; }

        [DataMember]
        public virtual string login_text_hex_color { get; set; }
    }

    [Route("/error")]
    [DataContract]
    public partial class ErrorPage
        : IReturn<Object>
    {
    }

    public partial class Event
    {
        public Event()
        {
            tags = new Tag[] { };
        }

        [DataMember]
        public virtual DateTime created_date { get; set; }

        [DataMember]
        public virtual string event_type { get; set; }

        [DataMember]
        public virtual string completion_state { get; set; }

        [DataMember]
        public virtual string email_address { get; set; }

        [DataMember]
        public virtual string provider_type { get; set; }

        [DataMember]
        public virtual string ip_address { get; set; }

        [DataMember]
        public virtual bool? event_success { get; set; }

        [DataMember]
        public virtual string local_success { get; set; }

        [DataMember]
        public virtual DateTime? local_success_date { get; set; }

        [DataMember]
        public virtual ValidationDetails validation_details { get; set; }

        [DataMember]
        public virtual string country_code { get; set; }

        [DataMember]
        public virtual string latitude { get; set; }

        [DataMember]
        public virtual string longitude { get; set; }

        [DataMember]
        public virtual Tag[] tags { get; set; }

        [DataMember]
        public virtual string operating_system { get; set; }

        [DataMember]
        public virtual string browser { get; set; }

        [DataMember]
        public virtual string device { get; set; }

        [DataMember]
        public virtual bool? is_bot { get; set; }
    }

    [Route("/events", "GET")]
    [DataContract]
    public partial class GetEvents
        : IReturn<GetEventsResponse>, IAppId
    {
        public GetEvents()
        {
            completion_states = new List<string> { };
        }

        [DataMember(IsRequired = true)]
        [ApiMember(IsRequired = true, ParameterType = "path")]
        public virtual string app_id { get; set; }

        ///<summary>
        ///Defaults to 1
        ///</summary>
        [DataMember]
        [ApiMember(Description = "Defaults to 1", ParameterType = "query")]
        public virtual int? page { get; set; }

        ///<summary>
        ///Defaults to 25
        ///</summary>
        [DataMember]
        [ApiMember(Description = "Defaults to 25", ParameterType = "query")]
        public virtual int? page_size { get; set; }

        [DataMember]
        [ApiMember(ParameterType = "query")]
        public virtual string start_date { get; set; }

        [DataMember]
        [ApiMember(ParameterType = "query")]
        public virtual string end_date { get; set; }

        [DataMember]
        [ApiMember(ParameterType = "query")]
        public virtual List<string> completion_states { get; set; }
    }

    public partial class GetEventsResponse
        : IBaseResponse
    {
        public GetEventsResponse()
        {
            events = new Event[] { };
        }

        [DataMember]
        public virtual IErrorResponse error { get; set; }

        [DataMember]
        public virtual int total_items { get; set; }

        [DataMember]
        public virtual int total_pages { get; set; }

        [DataMember]
        public virtual Event[] events { get; set; }
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
            social_identity_providers = new Provider[] { };
            enterprise_identity_providers = new EnterpriseProvider[] { };
        }

        [DataMember]
        public virtual Provider[] social_identity_providers { get; set; }

        [DataMember]
        public virtual EnterpriseProvider[] enterprise_identity_providers { get; set; }

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

    [Route("/session", "DELETE")]
    [DataContract]
    public partial class LogoutDelegatedIdPxSession
        : IReturn<LogoutDelegatedIdPxSessionResponse>, IAppId
    {
        [DataMember]
        [ApiMember(ParameterType = "query")]
        public virtual string app_id { get; set; }
    }

    [DataContract]
    public partial class LogoutDelegatedIdPxSessionResponse
        : IBaseResponse
    {
        [DataMember]
        public virtual IErrorResponse error { get; set; }
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

    [Route("/session", "POST")]
    [DataContract]
    public partial class SetDelegatedIdPxSession
        : IReturn<SetDelegatedIdPxSessionResponse>, IAppId
    {
        [DataMember]
        public virtual string app_id { get; set; }

        [DataMember]
        public virtual string session_token { get; set; }
    }

    [DataContract]
    public partial class SetDelegatedIdPxSessionResponse
        : IBaseResponse
    {
        [DataMember]
        public virtual IErrorResponse error { get; set; }
    }

    [Route("/start", "POST")]
    [DataContract]
    public partial class StartLogin
        : IReturn<StartLoginResponse>, IAppId
    {
        public StartLogin()
        {
            tags = new Tag[] { };
        }

        [DataMember(IsRequired = true)]
        public virtual string app_id { get; set; }

        [DataMember(IsRequired = true)]
        public virtual string identity_provider { get; set; }

        [DataMember]
        public virtual string identity_provider_id { get; set; }

        [DataMember]
        public virtual string email_address { get; set; }

        [DataMember]
        public virtual string client_data { get; set; }

        [DataMember]
        public virtual Tag[] tags { get; set; }

        [DataMember]
        public virtual string destination_url { get; set; }

        [DataMember]
        public virtual string callback_url { get; set; }
    }

    public partial class StartLoginResponse
        : IBaseResponse
    {
        public virtual IErrorResponse error { get; set; }
        [DataMember(IsRequired = true)]
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

    [Route("/events/{event_id}", "PUT")]
    [DataContract]
    public partial class UpdateEvent
        : IReturn<UpdateEventResponse>, IAppId
    {
        public UpdateEvent()
        {
            tags = new Tag[] { };
        }

        [DataMember(IsRequired = true)]
        public virtual string app_id { get; set; }

        [DataMember(IsRequired = true)]
        public virtual string event_id { get; set; }

        [DataMember]
        public virtual string local_validation { get; set; }

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
        public virtual bool event_success { get; set; }

        [DataMember]
        public virtual ValidationDetails validation_details { get; set; }

        [DataMember]
        public virtual IdentityProviderData identity_provider_data { get; set; }

        [DataMember]
        public virtual string client_data { get; set; }

        [DataMember]
        public virtual string event_id { get; set; }

        [DataMember]
        public virtual string destination_url { get; set; }

        public virtual IErrorResponse error { get; set; }
    }

    public partial class ValidationDetails
    {
        [DataMember]
        public virtual string domain_validation { get; set; }

        [DataMember]
        public virtual string ip_validation { get; set; }

        [DataMember]
        public virtual string geo_validation { get; set; }

        [DataMember]
        public virtual string time_validation { get; set; }
    }
}
