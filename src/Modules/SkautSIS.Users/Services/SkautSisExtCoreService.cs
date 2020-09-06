using Orchard;
using Orchard.ContentManagement;
using Orchard.Logging;
using SkautSIS.Core.Models;
using SkautSIS.Users.Models;
using System.Linq;
using SkautSIS.Users.Services.SkautIs.OrganizationUnit;
using System;

namespace SkautSIS.Users.Services
{
    public class SkautSisExtCoreService : ISkautSisExtCoreService
    {
        private readonly IWorkContextAccessor workContextAccessor;
        private readonly Lazy<SkautSisCoreSettingsPart> coreSettings;
        private readonly Lazy<OrganizationUnitSoapClient> organizationUnitClient;

        public SkautSisExtCoreService(
            IWorkContextAccessor workContextAccessor)
        {
            this.workContextAccessor = workContextAccessor;
            this.coreSettings = new Lazy<SkautSisCoreSettingsPart>(
                () => this.workContextAccessor.GetContext().CurrentSite.As<SkautSisCoreSettingsPart>());

            this.organizationUnitClient = new Lazy<OrganizationUnitSoapClient>(
                () => new OrganizationUnitSoapClient(
                    this.coreSettings.Value.SkautIsWsSoapBinding,
                    this.coreSettings.Value.SkautIsOrganizationUnitServiceAddress));

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void RefreshUnitInfo()
        {
            var workContext = this.workContextAccessor.GetContext();
            var userPart = workContext.CurrentUser.As<SkautIsUserPart>();
            var coreSettings = this.coreSettings.Value;
            var extCoreSettings = workContext.CurrentSite.As<SkautSisCoreExtSettingsPart>();
            
            var appId = coreSettings.AppId;
            var token = userPart.Token;
            var registrationNumber = extCoreSettings.UnitRegistrationNumber;

            if (String.IsNullOrEmpty(registrationNumber))
            {
                extCoreSettings.UnitRegistrationNumber = null;
                extCoreSettings.UnitId = null;
                extCoreSettings.UnitDisplayName = null;
                extCoreSettings.UnitTypeId = null;
            }
            else
            {
                if (appId.HasValue && token.HasValue)
                {
                    try
                    {
                        var unit = this.organizationUnitClient.Value.UnitAll(new UnitAllInput
                        {
                            ID_Application = appId.Value,
                            ID_Login = token.Value,
                            RegistrationNumber = registrationNumber
                        })
                        .FirstOrDefault();

                        if (unit != null)
                        {
                            extCoreSettings.UnitId = unit.ID.Value;
                            extCoreSettings.UnitDisplayName = unit.DisplayName;
                            extCoreSettings.UnitTypeId = unit.ID_UnitType;
                        }
                        else
                        {
                            extCoreSettings.UnitId = null;
                            extCoreSettings.UnitDisplayName = null;
                            extCoreSettings.UnitTypeId = null;
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
}