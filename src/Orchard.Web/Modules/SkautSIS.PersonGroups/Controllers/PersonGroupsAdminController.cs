using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard;
using Orchard.Core.Contents.Controllers;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Orchard.Utility.Extensions;
using SkautSIS.PersonGroups.Models;
using SkautSIS.PersonGroups.Services;
using SkautSIS.PersonGroups.ViewModels;
using SkautSIS.Users.Services;
using SkautSIS.Users.ViewModels;

namespace SkautSIS.PersonGroups.Controllers
{
    [Admin]
    [ValidateInput(false)]
    public class PersonGroupsAdminController : Controller
    {
        private readonly IOrchardServices _services;
        private readonly IPersonGroupsService _personGroupService;
        private readonly IUserCentralizationService _userCentralizationService;
        private readonly ISiteService _siteService;

        public PersonGroupsAdminController(
            IOrchardServices services,
            IShapeFactory shapeFactory,
            ISiteService siteService,
            IPersonGroupsService personGroupService,
            IUserCentralizationService userCentralizationService)
        {
            T = NullLocalizer.Instance;
            _services = services;
            Shape = shapeFactory;
            _siteService = siteService;
            _personGroupService = personGroupService;
            _userCentralizationService = userCentralizationService;
        }

        dynamic Shape { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index(PersonGroupIndexOptions options, PagerParameters pagerParameters)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManagePersonGroups, T("Not allowed to list person groups.")))
                return new HttpUnauthorizedResult();

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            if (options == null)
                options = new PersonGroupIndexOptions();

            var personGroups = _personGroupService.GetAll();

            switch (options.Filter)
            {
                case PersonGroupsFilter.Custom:
                    personGroups = personGroups.Where(p => p.Type == TypeEnum.Custom);
                    break;
                case PersonGroupsFilter.Unit:
                    personGroups = personGroups.Where(p => p.Type == TypeEnum.Unit);
                    break;
                case PersonGroupsFilter.Event:
                    personGroups = personGroups.Where(p => p.Type == TypeEnum.Event);
                    break;
            }

            if (!String.IsNullOrWhiteSpace(options.Search))
            {
                personGroups = personGroups.Where(p => p.DisplayName.ToLower().Contains(options.Search.ToLower()));
            }

            var pagerShape = Shape.Pager(pager).TotalItemCount(personGroups.Count());

            switch (options.Order)
            {
                case PersonGroupsOrder.Title:
                    personGroups = personGroups.OrderBy(p => p.DisplayName);
                    break;
                case PersonGroupsOrder.Type:
                    personGroups = personGroups.OrderBy(p => p.Type.ToString());
                    break;
            }

            var results = personGroups.Skip(pager.GetStartIndex()).Take(pager.PageSize).ToList();

            var model = new PersonGroupsIndexViewModel
            {
                PersonGroups = results
                    .Select(x => new PersonGroupEntry { PersonGroup = new PersonGroupModel {
                        DisplayName = x.DisplayName,
                        MemberSkautIsIds = x.MemberSkautIsIds,
                        PersonGroupId = x.PersonGroupId,
                        Type = x.Type
                    } })
                    .ToList(),
                Options = options,
                Pager = pagerShape
            };

            // maintain previous route data when generating page links
            var routeData = new RouteData();
            routeData.Values.Add("Options.Filter", options.Filter);
            routeData.Values.Add("Options.Search", options.Search);
            routeData.Values.Add("Options.Order", options.Order);

            pagerShape.RouteData(routeData);

            return View(model);
        }

        [HttpPost]
        [FormValueRequired("submit.BulkEdit")]
        public ActionResult Index(FormCollection input)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManagePersonGroups, T("Not allowed to list person groups.")))
                return new HttpUnauthorizedResult();

            var viewModel = new PersonGroupsIndexViewModel { PersonGroups = new List<PersonGroupEntry>(), Options = new PersonGroupIndexOptions() };
            UpdateModel(viewModel);

            var checkedEntries = viewModel.PersonGroups.Where(c => c.IsChecked);
            switch (viewModel.Options.BulkAction)
            {
                case PersonGroupsBulkAction.None:
                    break;
                case PersonGroupsBulkAction.Delete:
                    foreach (var entry in checkedEntries)
                    {
                        Delete(entry.PersonGroup.PersonGroupId);
                    }
                    break;
            }

            return RedirectToAction("Index", ControllerContext.RouteData.Values);
        }

        public ActionResult Details(int id)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManagePersonGroups, T("Not allowed to display detail of the person group.")))
                return new HttpUnauthorizedResult();

            var personGroup = _personGroupService.GetSingle(id);

            var model = new PersonGroupDetailsViewModel()
            {
                PersonGroup = personGroup,
                Members = personGroup.MemberSkautIsIds.Select(i => _userCentralizationService.GetUserCentralizedData(i)).ToList()
            };

            return View(model);
        }

        public ActionResult Create()
        {
            if (!_services.Authorizer.Authorize(Permissions.ManagePersonGroups, T("Not allowed to create new person group.")))
                return new HttpUnauthorizedResult();

            var model = new PersonGroupCreateViewModel
            {
                Persons = _userCentralizationService.GetUserAll().Select(x => new PersonEntry { Person = x }).ToList()
            };


            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST()
        {
            if (!_services.Authorizer.Authorize(Permissions.ManagePersonGroups, T("Not allowed to create new person group.")))
                return new HttpUnauthorizedResult();

            var model = new PersonGroupCreateViewModel { Persons = new List<PersonEntry>() };
            try
            {
                UpdateModel(model);

                if (!string.IsNullOrEmpty(model.DisplayName))
                {
                    if (!_personGroupService.VerifyUnicity(model.DisplayName))
                    {
                        ModelState.AddModelError("NotUniquePersonGroupName", T("Person group with that name already exists.").Text);
                    }
                }

                if (!ModelState.IsValid) return View(model);

                var checkedEntries = model.Persons.Where(c => c.IsChecked);

                var personGroup = new PersonGroupModel
                {
                    DisplayName = model.DisplayName,
                    Type = TypeEnum.Custom,
                    MemberSkautIsIds = checkedEntries.Select(p => p.Person.SkautIsPersonId).ToList()
                };

                _personGroupService.Create(personGroup);

                var pg = _personGroupService.GetSingle(model.DisplayName);
                _personGroupService.UpdateMembers(pg.PersonGroupId, checkedEntries.Select(p => p.Person.SkautIsPersonId.ToString()));

                _services.Notifier.Information(T("Person group created"));
                return RedirectToAction("Index");

            }
            catch (Exception exception)
            {
                _services.Notifier.Error(T("Creating person group failed: {0}", exception.Message));

                return RedirectToAction("Create");
            }
        }


        public ActionResult Edit(int id)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManagePersonGroups, T("Not allowed to create new person group.")))
                return new HttpUnauthorizedResult();

            var personGroup = _personGroupService.GetSingle(id);
            if (personGroup == null)
            {
                _services.Notifier.Error(T("{0} is not a valid person Id.", id));
                return RedirectToAction("Index");
            }

            var personEntries = _userCentralizationService.GetUserAll().Select(x => new PersonEntry { Person = x }).ToList();

            for (int i=0; i<personEntries.Count; i++) 
            {
                if (personGroup.MemberSkautIsIds.Contains(personEntries[i].Person.SkautIsPersonId))
                {
                    personEntries[i].IsChecked = true;
                }
            }

            var model = new PersonGroupEditViewModel
            {
                PersonGroupId = personGroup.PersonGroupId,
                DisplayName = personGroup.DisplayName,
                Persons = personEntries
            };

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPOST()
        {
            if (!_services.Authorizer.Authorize(Permissions.ManagePersonGroups, T("Not allowed to manage person groups.")))
                return new HttpUnauthorizedResult();

            var model = new PersonGroupEditViewModel { Persons = new List<PersonEntry>() };
            try
            {
                UpdateModel(model);

                if (!string.IsNullOrEmpty(model.DisplayName))
                {
                    if (!_personGroupService.VerifyUnicity(model.DisplayName, model.PersonGroupId))
                    {
                        ModelState.AddModelError("NotUniquePersonGroupName", T("Person group with that name already exists.").Text);
                    }
                }

                if (!ModelState.IsValid)
                    return RedirectToAction("Edit", model.PersonGroupId);

                var checkedEntries = model.Persons.Where(c => c.IsChecked);

                var personGroup = new PersonGroupModel
                {
                    PersonGroupId = model.PersonGroupId,
                    DisplayName = model.DisplayName,
                    Type = TypeEnum.Custom,
                    MemberSkautIsIds = checkedEntries.Select(p => p.Person.SkautIsPersonId).ToList()
                };

                _personGroupService.Update(personGroup);

                _services.Notifier.Information(T("Person group updated"));
                return RedirectToAction("Edit", model.PersonGroupId);

            }
            catch (Exception exception)
            {
                _services.Notifier.Error(T("Updating person group failed: {0}", exception.Message));

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(int id)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManagePersonGroups, T("Not allowed to display detail of the person group.")))
                return new HttpUnauthorizedResult();

            var group = _personGroupService.GetSingle(id);

            if(group == null) {
                _services.Notifier.Error(T("{0} is not a valid person group Id.", id));
                return RedirectToAction("Index");
            }

            if(group.Type != TypeEnum.Custom) 
            {
                _services.Notifier.Error(T("Person group {0} was not deleted. Automatic person groups cannot be deleted.", group.DisplayName));
                return RedirectToAction("Index");
            }

            _personGroupService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}