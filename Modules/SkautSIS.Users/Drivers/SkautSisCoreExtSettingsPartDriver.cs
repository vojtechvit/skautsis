using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Security;
using SkautSIS.Users.Models;
using SkautSIS.Users.Services;
using SkautSIS.Users.ViewModels;

namespace SkautSIS.Users.Drivers
{
    public class SkautSisCoreExtSettingsPartDriver : ContentPartDriver<SkautSisCoreExtSettingsPart>
    {
        private readonly ISkautIsUserService userService;
        private readonly ISkautSisExtCoreService extCoreService;

        public SkautSisCoreExtSettingsPartDriver(
            ISkautIsUserService userService,
            ISkautSisExtCoreService extCoreService)
        {
            this.userService = userService;
            this.extCoreService = extCoreService;
        }

        protected override string Prefix { get { return "SkautSisUsersCoreExtSettings"; } }

        protected override DriverResult Editor(SkautSisCoreExtSettingsPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(SkautSisCoreExtSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_SkautSis_Users_CoreExtSettings", () =>
            {
                var viewModel = new EditSkautSisCoreExtSettingsViewModel
                {
                    ExtSettingsPart = part,
                    SkautIsUserNeeded = !this.userService.IsSkautIsUser()
                };

                if (updater != null && this.userService.IsSkautIsUser())
                {
                    this.extCoreService.RefreshUnitInfo();

                    // updater.TryUpdateModel(part, Prefix, null, null);
                }

                return shapeHelper.EditorTemplate(TemplateName: "Parts/SkautSis.Users.CoreExtSettings", Model: viewModel, Prefix: Prefix);
            })
            .OnGroup("SkautSIS");
        }

        protected override void Importing(SkautSisCoreExtSettingsPart part, ImportContentContext context)
        {
            var unitTypeId = context.Attribute(part.PartDefinition.Name, "UnitTypeId");

            if (unitTypeId != null) part.UnitTypeId = unitTypeId;
        }

        protected override void Exporting(SkautSisCoreExtSettingsPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("UnitTypeId", part.Record.UnitTypeId);
        }
    }
}