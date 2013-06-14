using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using SkautSIS.PersonGroups.Models;
using SkautSIS.PersonGroups.Services;
using SkautSIS.PersonGroups.ViewModels;

namespace SkautSIS.PersonGroups.Drivers
{
    public class PersonGroupsVisibilityPartDriver : ContentPartDriver<PersonGroupsVisibilityPart>
    {
        private readonly IPersonGroupsVisibilityService _groupsVisibilityService;
        private readonly IPersonGroupsService _personGroupsService;

        [UsedImplicitly]
        public PersonGroupsVisibilityPartDriver(
            IPersonGroupsVisibilityService groupsVisibilityService,
            IPersonGroupsService personGroupsService)
        {
            _groupsVisibilityService = groupsVisibilityService;
            _personGroupsService = personGroupsService;
        }

        protected override string Prefix
        {
            get { return "PersonGroupsVisibility"; }
        }

        protected override DriverResult Display(
            PersonGroupsVisibilityPart part,
            string displayType,
            dynamic shapeHelper) 
        {
            return ContentShape("Parts_PersonGroupsVisibility",
                () => shapeHelper.Parts_PersonGroupsVisibility(
                                ContentPart: part,
                                PersonGroups: part.PersonGroupIds));
        }

        protected override DriverResult Editor(
            PersonGroupsVisibilityPart part,
            dynamic shapeHelper)
        {

            return ContentShape("Parts_PersonGroupsVisibility_Edit",
                    () => shapeHelper.EditorTemplate(
                        TemplateName: "Parts/PersonGroupsVisibility",
                        Model: BuildEditorViewModel(part),
                        Prefix: Prefix));
        }

        protected override DriverResult Editor(
            PersonGroupsVisibilityPart part,
            IUpdateModel updater,
            dynamic shapeHelper)
        {

            var model = new EditPersonGroupsVisibilityViewModel();
            updater.TryUpdateModel(model, Prefix, null, null);

            if (part.ContentItem.Id != 0)
            {
                _groupsVisibilityService.UpdatePersonGroupsForContentItem(
                    part.ContentItem, model.PersonGroups);
            }

            return Editor(part, shapeHelper);
        }

        private EditPersonGroupsVisibilityViewModel BuildEditorViewModel(PersonGroupsVisibilityPart part)
        {
            var item = part.PersonGroupIds;
            return new EditPersonGroupsVisibilityViewModel
            {
                PersonGroups = _personGroupsService.GetAll().Select(
                    x => new PersonGroupEntry
                    {
                        PersonGroup = new PersonGroupModel
                        {
                            DisplayName = x.DisplayName,
                            MemberSkautIsIds = x.MemberSkautIsIds,
                            PersonGroupId = x.PersonGroupId,
                            Type = x.Type
                        },
                        IsChecked = item.Contains(x.PersonGroupId)
                    }).ToList()
            };
        }
    }
}