using Orchard.ContentManagement;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SkautSIS.Core.Models
{
    public class SkautSisCoreSettingsPart : ContentPart
    {
        private const string SkautIsProductionUrl = "https://is.skaut.cz/";
        private const string SkautIsTestingUrl = "http://test-is.skaut.cz/";

        private const string SkautIsProductionWsBaseUrl = "https://is.skaut.cz/JunakWebservice/";
        private const string SkautIsTestingWsBaseUrl = "http://test-is.skaut.cz/JunakWebservice/";

        private const string SkautIsUserManagementServicePath = "UserManagement.asmx";
        private const string SkautIsOrganizationUnitServicePath = "OrganizationUnit.asmx";

        private static Binding SkautIsProductionWsSoapBinding = new BasicHttpBinding
        {
            Name = "SkautIsProductionWsSoapBinding",
            Security = new BasicHttpSecurity
            {
                Mode = BasicHttpSecurityMode.Transport
            }
        };

        private static Binding SkautIsTestingWsSoapBinding = new BasicHttpBinding
        {
            Name = "SkautIsTestingWsSoapBinding",
            Security = new BasicHttpSecurity
            {
                Mode = BasicHttpSecurityMode.None
            }
        };

        public Guid? AppId
        {
            get { return string.IsNullOrEmpty(this.Retrieve<string>("AppId")) ? (Guid?)null : Guid.Parse(this.Retrieve<string>("AppId")); }
            set { this.Store<string>("AppId", value.HasValue ? value.Value.ToString() : null); }
        }

        public bool UseTestingWebServices
        {
            get { return this.Retrieve(x => x.UseTestingWebServices); }
            set { this.Store(x => x.UseTestingWebServices, value); }
        }

        public string SkautIsUrl
        {
            get
            {
                return UseTestingWebServices ? SkautIsTestingUrl : SkautIsProductionUrl;
            }
        }

        public string SkautIsWsBaseAddress
        {
            get
            {
                return UseTestingWebServices ? SkautIsTestingWsBaseUrl : SkautIsProductionWsBaseUrl;
            }
        }

        public EndpointAddress SkautIsUserManagementServiceAddress
        {
            get
            {
                return new EndpointAddress(this.SkautIsWsBaseAddress + SkautIsUserManagementServicePath);
            }
        }

        public EndpointAddress SkautIsOrganizationUnitServiceAddress
        {
            get
            {
                return new EndpointAddress(this.SkautIsWsBaseAddress + SkautIsOrganizationUnitServicePath);
            }
        }

        public Binding SkautIsWsSoapBinding
        {
            get
            {
                return UseTestingWebServices ? SkautIsTestingWsSoapBinding : SkautIsProductionWsSoapBinding;
            }
        }
    }
}