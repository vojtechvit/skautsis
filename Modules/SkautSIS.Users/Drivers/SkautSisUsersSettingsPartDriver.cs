using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Logging;
using Orchard.Roles.Services;
using SkautSIS.Core.Models;
using SkautSIS.Users.Models;
using SkautSIS.Users.Services.SkautIs.UserManagement;
using SkautSIS.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SkautSIS.Users.Drivers
{
    public class SkautSisUsersSettingsPartDriver : ContentPartDriver<SkautSisUsersSettingsPart>
    {
        private readonly IWorkContextAccessor workContextAccessor;
        private readonly IRoleService roleService;
        private readonly Lazy<SkautSisCoreSettingsPart> coreSettings;
        private readonly Lazy<SkautSisCoreExtSettingsPart> coreExtSettings;
        private readonly Lazy<UserManagementSoapClient> userManagementClient;

        public SkautSisUsersSettingsPartDriver(
            IWorkContextAccessor workContextAccessor,
            IRoleService roleService)
        {
            this.workContextAccessor = workContextAccessor;
            this.roleService = roleService;

            this.coreSettings = new Lazy<SkautSisCoreSettingsPart>(
                () => this.workContextAccessor.GetContext().CurrentSite.As<SkautSisCoreSettingsPart>());
            this.coreExtSettings = new Lazy<SkautSisCoreExtSettingsPart>(
                () => this.workContextAccessor.GetContext().CurrentSite.As<SkautSisCoreExtSettingsPart>());

            this.userManagementClient = new Lazy<UserManagementSoapClient>(
                () => new UserManagementSoapClient(
                    this.coreSettings.Value.SkautIsWsSoapBinding,
                    this.coreSettings.Value.SkautIsUserManagementServiceAddress));

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        protected override string Prefix { get { return "SkautSisUsersSettings"; } }

        protected override DriverResult Editor(SkautSisUsersSettingsPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(SkautSisUsersSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_SkautSis_Users_Settings", () =>
            {
                var workContext = this.workContextAccessor.GetContext();
                var userPart = workContext.CurrentUser.As<SkautIsUserPart>();

                var viewModel = new EditSkautSisUsersSettingsViewModel();

                if (!this.coreSettings.Value.AppId.HasValue || !userPart.SkautIsUserId.HasValue)
                {
                    viewModel.SkautIsUserNeeded = true;
                }
                else
                {
                    viewModel.SettingsPart = part;
                    viewModel.LocalRoles = this.roleService.GetRoles().Select(r => new SelectListItem
                        {
                            Text = r.Name,
                            Value = r.Id.ToString()
                        });

                    try
                    {
                        var roles = this.userManagementClient.Value.RoleAll(new RoleAllInput
                            {
                                ID_Login = userPart.Token.Value,
                                ID_UnitType = this.coreExtSettings.Value.UnitTypeId
                            });

                        viewModel.SkautIsRoles = roles.Select(r => new SelectListItem
                        {
                            Text = r.DisplayName,
                            Value = r.ID.Value.ToString()
                        });
                    }
                    catch (Exception e)
                    {
                        Logger.Debug(e, e.Message);
                    }

                    if (updater != null)
                    {
                        updater.TryUpdateModel(viewModel, Prefix, null, null);
                    }
                }

                return shapeHelper.EditorTemplate(TemplateName: "Parts/SkautSis.Users.Settings", Model: viewModel, Prefix: Prefix); 
            })
            .OnGroup("SkautSIS Users");
        }

        protected override void Importing(SkautSisUsersSettingsPart part, ImportContentContext context)
        {
            var rolesAssignedByMembership = context.Attribute(part.PartDefinition.Name, "RolesAssignedByMembership");
            var rolesAssignedBySkautIsRoles = context.Attribute(part.PartDefinition.Name, "RolesAssignedBySkautIsRoles");

            if (rolesAssignedByMembership != null) part.RolesAssignedByMembership = rolesAssignedByMembership;
            if (rolesAssignedBySkautIsRoles != null) part.RolesAssignedBySkautIsRoles = rolesAssignedBySkautIsRoles;
        }

        protected override void Exporting(SkautSisUsersSettingsPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("RolesAssignedByMembership", part.Record.RolesAssignedByMembership);
            context.Element(part.PartDefinition.Name).SetAttributeValue("RolesAssignedBySkautIsRoles", part.Record.RolesAssignedBySkautIsRoles);
        }
    }
}