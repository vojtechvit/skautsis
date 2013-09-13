using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Shapes.Localization;
using SkautSIS.Users.Models;
using SkautSIS.Users.ViewModels;
using System;

namespace SkautSIS.Users.Drivers
{
    public class SkautIsUserPartDriver : ContentPartDriver<SkautIsUserPart>
    {
        private readonly Lazy<IDateTimeLocalization> dateTimeLocalization;

        public SkautIsUserPartDriver(Lazy<IDateTimeLocalization> dateTimeLocalization)
        {
            this.dateTimeLocalization = dateTimeLocalization;
        }

        protected override string Prefix { get { return "SkautSisUsersUser"; } }

        protected override DriverResult Editor(SkautIsUserPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(SkautIsUserPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_SkautSis_Users_User_Edit", () =>
            {
                if (updater != null)
                {
                    // Properties of this part are changed automatically elsewhere
                    // updater.TryUpdateModel(part, Prefix, null, null);
                }

                var viewModel = new EditSkautIsUserViewModel
                {
                    UserPart = part,
                    DateTimeLocalization = this.dateTimeLocalization.Value
                };

                return shapeHelper.EditorTemplate(TemplateName: "Parts/SkautSis.Users.User", Model: viewModel, Prefix: Prefix);
            });
        }
        
        protected override void Importing(SkautIsUserPart part, ImportContentContext context)
        {
            var skautIsUserId = context.Attribute(part.PartDefinition.Name, "SkautIsUserId");
            var skautIsUserName = context.Attribute(part.PartDefinition.Name, "SkautIsUserName");
            var skautIsPersonId = context.Attribute(part.PartDefinition.Name, "PersonId");
            var firstName = context.Attribute(part.PartDefinition.Name, "FirstName");
            var lastName = context.Attribute(part.PartDefinition.Name, "LastName");
            var nickName = context.Attribute(part.PartDefinition.Name, "NickName");
            var birthDate = context.Attribute(part.PartDefinition.Name, "BirthDate");

            if (skautIsUserId != null) part.SkautIsUserId = int.Parse(skautIsUserId);
            if (skautIsUserName != null) part.SkautIsUserName = skautIsUserName;
            if (skautIsPersonId != null) part.PersonId = int.Parse(skautIsPersonId);
            if (firstName != null) part.FirstName = firstName;
            if (lastName != null) part.LastName = lastName;
            if (nickName != null) part.NickName = nickName;
            if (birthDate != null) part.BirthDate = DateTime.Parse(birthDate);
        }

        protected override void Exporting(SkautIsUserPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("SkautIsUserId", part.Record.SkautIsUserId);
            context.Element(part.PartDefinition.Name).SetAttributeValue("SkautIsUserName", part.Record.SkautIsUserName);
            context.Element(part.PartDefinition.Name).SetAttributeValue("PersonId", part.Record.PersonId);
            context.Element(part.PartDefinition.Name).SetAttributeValue("FirstName", part.Record.FirstName);
            context.Element(part.PartDefinition.Name).SetAttributeValue("LastName", part.Record.LastName);
            context.Element(part.PartDefinition.Name).SetAttributeValue("NickName", part.Record.NickName);
            context.Element(part.PartDefinition.Name).SetAttributeValue("BirthDate", part.Record.BirthDate);
        }
    }
}