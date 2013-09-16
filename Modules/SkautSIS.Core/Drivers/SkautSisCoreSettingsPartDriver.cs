using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using SkautSIS.Core.Models;
using System;

namespace SkautSIS.Core.Drivers
{
    public class SkautSisCoreSettingsPartDriver : ContentPartDriver<SkautSisCoreSettingsPart>
    {
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
                    updater.TryUpdateModel(part, Prefix, null, null);
                }

                return shapeHelper.EditorTemplate(TemplateName: "Parts/SkautSis.Core.Settings", Model: part, Prefix: Prefix); 
            })
            .OnGroup("SkautSIS");
        }

        protected override void Importing(SkautSisCoreSettingsPart part, ImportContentContext context)
        {
            var appId = context.Attribute(part.PartDefinition.Name, "AppId");
            var useTestingWebServices = context.Attribute(part.PartDefinition.Name, "UseTestingWebServices");

            if (appId != null) part.AppId = Guid.Parse(appId);
            if (useTestingWebServices != null) part.UseTestingWebServices = bool.Parse(useTestingWebServices);
        }

        protected override void Exporting(SkautSisCoreSettingsPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("AppId", part.Record.AppId);
            context.Element(part.PartDefinition.Name).SetAttributeValue("UseTestingWebServices", part.Record.UseTestingWebServices);
        }
    }
}