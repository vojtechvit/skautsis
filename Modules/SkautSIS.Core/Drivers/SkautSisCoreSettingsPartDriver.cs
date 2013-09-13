using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using SkautSIS.Core.Models;
using SkautSIS.Core.Services;
using System;

namespace SkautSIS.Core.Drivers
{
    public class SkautSisCoreSettingsPartDriver : ContentPartDriver<SkautSisCoreSettingsPart>
    {
        private readonly ISkautSisCoreService coreService;

        public SkautSisCoreSettingsPartDriver(ISkautSisCoreService coreService)
        {
            this.coreService = coreService;
        }

        protected override string Prefix { get { return "SkautSisCoreSettings"; } }

        protected override DriverResult Editor(SkautSisCoreSettingsPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(SkautSisCoreSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_SkautSis_Core_Settings", () =>
            {
                if (updater != null)
                {
                    if (updater.TryUpdateModel(part, Prefix, null, new[] { "UnitId", "UnitTypeId", "UnitDisplayName" }))
                    {
                        this.coreService.RefreshUnitInfo();
                    }
                }

                return shapeHelper.EditorTemplate(TemplateName: "Parts/SkautSis.Core.Settings", Model: part, Prefix: Prefix); 
            })
            .OnGroup("SkautSIS");
        }

        protected override void Importing(SkautSisCoreSettingsPart part, ImportContentContext context)
        {
            var appId = context.Attribute(part.PartDefinition.Name, "AppId");
            var useTestingWebServices = context.Attribute(part.PartDefinition.Name, "UseTestingWebServices");
            var unitId = context.Attribute(part.PartDefinition.Name, "UnitId");
            var unitRegistrationNumber = context.Attribute(part.PartDefinition.Name, "UnitRegistrationNumber");
            var unitDisplayName = context.Attribute(part.PartDefinition.Name, "UnitDisplayName");

            if (appId != null) part.AppId = Guid.Parse(appId);
            if (useTestingWebServices != null) part.UseTestingWebServices = bool.Parse(useTestingWebServices);
            if (unitId != null) part.UnitId = int.Parse(unitId);
            if (unitRegistrationNumber != null) part.UnitRegistrationNumber = unitRegistrationNumber;
            if (unitDisplayName != null) part.UnitDisplayName = unitDisplayName;
        }

        protected override void Exporting(SkautSisCoreSettingsPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("AppId", part.Record.AppId);
            context.Element(part.PartDefinition.Name).SetAttributeValue("UseTestingWebServices", part.Record.UseTestingWebServices);
            context.Element(part.PartDefinition.Name).SetAttributeValue("UnitId", part.Record.UnitId);
            context.Element(part.PartDefinition.Name).SetAttributeValue("UnitRegistrationNumber", part.Record.UnitRegistrationNumber);
            context.Element(part.PartDefinition.Name).SetAttributeValue("UnitDisplayName", part.Record.UnitDisplayName);
        }
    }
}