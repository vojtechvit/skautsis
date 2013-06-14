using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Contents.Controllers;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using Orchard.Mvc.Extensions;
using System;
using Orchard.Settings;
using Orchard.UI.Navigation;
using SkautSIS.Users.Models;
using SkautSIS.Users.Services;
using SkautSIS.Users.ViewModels;
using CreateUserParams = SkautSIS.Users.Models.CreateUserParams;

namespace SkautSIS.Users.Controllers
{
    [Admin]
    public class AccountAdminController : Controller, IUpdateModel
    {
        private readonly IOrchardServices _orchardServices;
        private readonly ISiteService _siteService;
        private readonly IUserCentralizationService _userCentralService;
        private readonly ISkautIsUserService _skautIsUserService;
        private readonly ISkautIsMembershipService _membershipService;

        public Localizer T { get; set; }
        dynamic Shape { get; set; }
        
        public AccountAdminController(
            IOrchardServices orchardServices,
            ISiteService siteService,
            IUserCentralizationService userCentralService,
            IShapeFactory shapeFactory,
            ISkautIsMembershipService membershipService,
            ISkautIsUserService skautIsUserService) 
        {
            _orchardServices = orchardServices;
            _siteService = siteService;
            T = NullLocalizer.Instance;
            _userCentralService = userCentralService;
            _skautIsUserService = skautIsUserService;
            _membershipService = membershipService;

            Shape = shapeFactory;
        }

        public ActionResult Index(UserIndexOptions options)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageUsers, T("Not authorized to list users")))
                return new HttpUnauthorizedResult();


            if (options == null)
                options = new UserIndexOptions();

            IEnumerable<SkautIsUserPart> users = _orchardServices.ContentManager.Query<SkautIsUserPart, SkautIsUserPartRecord>().List();

            IEnumerable<SkautIsUserCentralizedParams> userParams = users.Select(x => _userCentralService.GetUserCentralizedData(x.Record.SkautIsPersonId));

            // filter?)

            if (!String.IsNullOrWhiteSpace(options.Search)) 
            {
                userParams = userParams.Where(u => u.FirstName.ToLower().Contains(options.Search.ToLower()) 
                    || u.LastName.ToLower().Contains(options.Search.ToLower())
                    || u.NickName.ToLower().Contains(options.Search.ToLower()));
            }
            
            switch (options.Order) 
            {
                case UsersOrder.Name:
                    userParams = userParams.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ThenBy(u => u.NickName);
                    break;
                case UsersOrder.NickName:
                    userParams = userParams.OrderBy(u => u.NickName).ThenBy(u => u.LastName);
                    break;
                case UsersOrder.Email:
                    userParams = userParams.OrderBy(u => u.Email).ThenBy(u => u.LastName);
                    break;
            }
            
            var model = new UsersIndexViewModel
            {
                Users = userParams
                    .Select(x => new UserEntry { User = x })
                    .ToList(),
                Options = options
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection input) 
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageUsers, T("Not authorized to manage users")))
                return new HttpUnauthorizedResult();

            var viewModel = new UsersIndexViewModel { Users = new List<UserEntry>(), Options = new UserIndexOptions() };
            UpdateModel(viewModel);
            
            return RedirectToAction("Index", ControllerContext.RouteData.Values);
        }

        public ActionResult Create()
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageUsers, T("Not authorized to manage users")))
                return new HttpUnauthorizedResult();

            var user = _orchardServices.ContentManager.New<IUser>("User");
            var editor = Shape.EditorTemplate(TemplateName: "Parts/SkautIsUser.Create", Model: new UserCreateViewModel(), Prefix: null);
            editor.Metadata.Position = "2";
            dynamic model = _orchardServices.ContentManager.BuildEditor(user);
            model.Content.Add(editor);

            return View((object)model);
        }


        [HttpPost]
        public ActionResult Create(UserCreateViewModel createModel)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageUsers, T("Not authorized to manage users")))
                return new HttpUnauthorizedResult();

            if (!string.IsNullOrEmpty(createModel.UserName))
            {
                if (!_skautIsUserService.VerifyUserUnicity(createModel.UserName, createModel.Email))
                {
                    AddModelError("NotUniqueUserName", T("User with that username and/or email already exists."));
                }
            }

            if (!Regex.IsMatch(createModel.Email ?? "", SkautIsUserPart.EmailPattern, RegexOptions.IgnoreCase))
            {
                AddModelError("Email", T("You must specify a valid email address."));
            }

            if (createModel.Password != createModel.ConfirmPassword)
            {
                AddModelError("ConfirmPassword", T("Password confirmation must match"));
            }

            int? userId = null;
            if (ModelState.IsValid) {
                try {
                    userId = _skautIsUserService.CreateSkautIsUser(
                        new CreateUserParams {
                            UserName = createModel.UserName,
                            Password = createModel.Password,
                            Email = createModel.Email,
                            LastName = createModel.LastName,
                            FirstName = createModel.FirstName,
                            NickName = createModel.NickName
                        });
                }
                catch (Exception e) {
                    AddModelError("SkautIsError", T("System SkautIS raised an exception."));
                    if (e.Message.Equals("Chyba validace (User_UserNameAlreadyExists)"))
                        _orchardServices.Notifier.Information(T("User name already exists."));

                }
            }

            if (ModelState.IsValid) {
                if (userId == null) {
                    throw new Exception("Cannot insert new user into SkautIS.");
                }

                var personId = _skautIsUserService.GetSkautIsPersonId(_orchardServices.WorkContext.CurrentUser.As<SkautIsUserPart>().Record.SkautIsToken.Value, userId.Value);

                var user = _membershipService.CreateUser(new SkautIsUserParams {
                    UserName = createModel.UserName,
                    Email = createModel.Email,
                    LastName = createModel.LastName,
                    FirstName = createModel.FirstName,
                    NickName = createModel.NickName,
                    SkautIsUserId = userId.Value,
                    SkautIsPersonId = personId.Value
                });
            }

            if (!ModelState.IsValid) 
            {
                var u = _orchardServices.ContentManager.New<IUser>("User");
                var editor = Shape.EditorTemplate(TemplateName: "Parts/SkautIsUser.Create", Model: createModel, Prefix: null);
                editor.Metadata.Position = "2";
                dynamic model = _orchardServices.ContentManager.BuildEditor(u);
                model.Content.Add(editor);

                return View((object)model);
            }

            _orchardServices.Notifier.Information(T("User created"));
            return RedirectToAction("Index");
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        public void AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}