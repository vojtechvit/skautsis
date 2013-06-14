using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.Localization;
using Orchard.Themes;
using SkautSIS.Core.Models;
using SkautSIS.Core.Services;
using SkautSIS.Users.Models;
using SkautSIS.Users.Services;
using CreateUserParams = SkautSIS.Users.Models.CreateUserParams;

namespace SkautSIS.Users.Controllers
{
    using System.Web.Mvc;

    [HandleError, Themed]
    public class AccountController : Controller 
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOrchardServices _orchardServices;
        private readonly IUserService _userService;
        private readonly ISkautIsUserService _skautIsUserService;

        public AccountController(
            IAuthenticationService authenticationService,
            IOrchardServices orchardServices,
            IUserService userService,
            ISkautIsUserService skautIsUserService) 
        {
            _authenticationService = authenticationService;
            _orchardServices = orchardServices;
            _userService = userService;
            
            T = NullLocalizer.Instance;

            _skautIsUserService = skautIsUserService;
        }

        public Localizer T { get; set; }

        public ActionResult AccessDenied() 
        {
            var currentUser = _authenticationService.GetAuthenticatedUser();

            if (currentUser == null)
            {
                var shape = _orchardServices.New.LogOn().Title(T("Access Denied").Text);
                return new ShapeResult(this, shape);
            }

            return View();
        }

        public ActionResult LogOn() 
        {
            if (_authenticationService.GetAuthenticatedUser() != null)
            {
                return Redirect("~/");
            }
            
            return Redirect(String.Format(
                "{0}Login/?appid={1}",
                "http://test-is.skaut.cz/",
                "70b0225b-4230-4a55-b4ac-de0ca4a56749"));

        }

        [HttpPost, ActionName("LogOn")]
        public ActionResult LogOnPOST() 
        {
            Guid? token = null;
            int? unitId = null, roleId = null;
            try {
                token = new Guid(Request.Form["skautIS_Token"]);
                unitId = int.Parse(Request.Form["skautIS_IDUnit"]);
                roleId = int.Parse(Request.Form["skautIS_IDRole"]);
            }
            catch {}

            if (token == null) RedirectToLogOnPage();
            var user = _userService.GetUser(token.Value, roleId, unitId);
            if (user == null) RedirectToLogOnPage();
            _authenticationService.SignIn(user, false);

            return Redirect("~/");
        }

        private ActionResult RedirectToLogOnPage() {
            var shape = this._orchardServices.New.LogOn().Title(T("Sign in was unsuccessfull, please try it agani. If the problem remains, contact the webmaster.").Text);
            return new ShapeResult(this, shape);
        }

        public ActionResult LogOff() 
        {
            var user = _authenticationService.GetAuthenticatedUser();
           
            if (user != null) 
            {   
                _authenticationService.SignOut();

                // TODO Opravit odhlaseni z skautIS, az prjde odpoved co bylo zmeneno
                //var token = user.As<SkautIsUserPart>().Record.SkautIsToken;
                //if (token.HasValue) _skautIsUserService.LogOut(user.As<SkautIsUserPart>().Record.SkautIsToken.Value);
            }

            return Redirect("~/");
        }

        public ActionResult LoggedOff()
        {
            var shape = this._orchardServices.New.LoggedOff().Title(T("You were successfully logged off").Text);
            return new ShapeResult(this, shape);
        }

        public ActionResult Register() 
        {
            // ensure users can register
            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
            if (!registrationSettings.UsersCanRegister)
            {
                return HttpNotFound();
            }

            var shape = _orchardServices.New.Register().Title(T("Create a New Account").Text);
            return new ShapeResult(this, shape); 
        }

        [HttpPost]
        public ActionResult Register(string userName, string email, string password, string confirmPassword, string firstname, string lastname, string nickname) 
        {
            // ensure users can register
            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
            if (!registrationSettings.UsersCanRegister)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(userName))
            {
                if (!_skautIsUserService.VerifyUserUnicity(userName, email))
                {
                    ModelState.AddModelError("NotUniqueUserName", T("User with that username and/or email already exists."));
                }
            }

            if (!Regex.IsMatch(email ?? "", SkautIsUserPart.EmailPattern, RegexOptions.IgnoreCase))
            {
                ModelState.AddModelError("Email", T("You must specify a valid email address."));
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", T("Password confirmation must match"));
            }

            int? userId = null;
            if (ModelState.IsValid)
            {
                try
                {
                    userId = _skautIsUserService.CreateSkautIsUser(
                        new CreateUserParams
                        {
                            UserName = userName,
                            Password = password,
                            Email = email,
                            LastName = lastname,
                            FirstName = firstname,
                            NickName = nickname
                        });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("SkautIsError", T("System SkautIS raised an exception."));
                    if (e.Message.Equals("Chyba validace (User_UserNameAlreadyExists)"))
                        ModelState.AddModelError("SkautIsUserExists", T("User name already exists."));
                }
            }

            if (ModelState.IsValid)
            {
                if (userId == null)
                {
                    throw new Exception("Cannot insert new user into SkautIS.");
                }

                // Cannot instert to SkautSIS, skatIS person ID unknown
                //var user = _membershipService.CreateUser(new SkautIsUserParams {
                //    UserName = userName,
                //    Email = email,
                //    LastName = lastname,
                //    FirstName = firstname,
                //    NickName = nickname,
                //    SkautIsUserId = userId.Value
                //});
                //_authenticationService.SignIn(user, false /* createPersistentCookie */);
                
                return RedirectToAction("LogOn");
            }

            // If we got this far, something failed, redisplay form
            var shape = _orchardServices.New.Register();
            return new ShapeResult(this, shape); 
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                return View();
            }

            try
            {
                //var validated = _membershipService.ValidateUser(User.Identity.Name, currentPassword);

                //if (validated != null)
                //{
                //    _membershipService.SetPassword(validated, newPassword);
                //    foreach (var userEventHandler in _userEventHandlers)
                //    {
                //        userEventHandler.ChangedPassword(validated);
                //    }
                //    return RedirectToAction("ChangePasswordSuccess");
                //}

                ModelState.AddModelError("_FORM", T("The current password is incorrect or the new password is invalid."));
                return ChangePassword();
            }
            catch
            {
                ModelState.AddModelError("_FORM", T("The current password is incorrect or the new password is invalid."));
                return ChangePassword();
            }
        }
    }
}