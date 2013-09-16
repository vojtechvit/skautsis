using Orchard.ContentManagement;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SkautSIS.Core.Models
{
    public class SkautSisCoreSettingsPart : ContentPart<SkautSisCoreSettingsPartRecord>
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
            get { return this.Record.AppId; }
            set { this.Record.AppId = value; }
        }

        public bool UseTestingWebServices
        {
            get { return this.Record.UseTestingWebServices; }
            set { this.Record.UseTestingWebServices = value; }
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