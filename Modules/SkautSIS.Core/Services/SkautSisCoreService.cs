using Orchard;
using Orchard.ContentManagement;
using Orchard.Logging;
using SkautSIS.Core.Models;
using SkautSIS.Core.Services.SkautIS.OrganizationUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkautSIS.Core.Services
{
    public class SkautSisCoreService : ISkautSisCoreService
    {
        private readonly IOrchardServices orchardServices;
        private readonly Lazy<SkautSisCoreSettingsPart> coreSettings;
        private readonly Lazy<OrganizationUnitSoapClient> organizationUnitClient;

        public SkautSisCoreService(
            IOrchardServices orchardServices)
        {
            this.orchardServices = orchardServices;
            this.coreSettings = new Lazy<SkautSisCoreSettingsPart>(
                () => this.orchardServices.WorkContext.CurrentSite.As<SkautSisCoreSettingsPart>());

            this.organizationUnitClient = new Lazy<OrganizationUnitSoapClient>(
                () => new OrganizationUnitSoapClient(
                    this.coreSettings.Value.SkautIsWsSoapBinding,
                    this.coreSettings.Value.SkautIsOrganizationUnitServiceAddress));

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void RefreshUnitInfo()
        {
            var coreSettings = this.coreSettings.Value;
            var appId = coreSettings.AppId;
            var registrationNumber = coreSettings.UnitRegistrationNumber;

            if (String.IsNullOrEmpty(registrationNumber))
            {
                coreSettings.UnitRegistrationNumber = null;
                coreSettings.UnitId = null;
                coreSettings.UnitDisplayName = null;
            }
            else
            {
                try
                {
                    var unit = this.organizationUnitClient.Value.UnitAllRegistry(new UnitAllRegistryInput
                    {
                        ID_Application = appId,
                        RegistrationNumber = registrationNumber
                    })
                    .FirstOrDefault();

                    if (unit != null)
                    {
                        coreSettings.UnitId = unit.ID.Value;
                        coreSettings.UnitDisplayName = unit.DisplayName;
                    }
                }
                catch (Exception e)
                {
                    Logger.Debug(e, e.Message);
                }
            }
        }
    }
}