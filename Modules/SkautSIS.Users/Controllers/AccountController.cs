using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.Themes;
using Orchard.UI.Notify;
using SkautSIS.Core.Models;
using SkautSIS.Users.Models;
using SkautSIS.Users.Services;
using System;
using System.Web.Mvc;

namespace SkautSIS.Users.Controllers
{
    [Themed]
    public class AccountController : Controller 
    {
        private readonly INotifier notifier;
        private readonly IAuthenticationService authenticationService;
        private readonly IOrchardServices orchardServices;
        private readonly ISkautIsUserService userService;
        private readonly SkautSisCoreSettingsPart coreSettings;

        public AccountController(
            INotifier notifier,
            IAuthenticationService authenticationService,
            IOrchardServices orchardServices,
            ISkautIsUserService userService)
        {
            this.notifier = notifier;
            this.authenticationService = authenticationService;
            this.orchardServices = orchardServices;
            this.userService = userService;
            this.coreSettings = orchardServices.WorkContext.CurrentSite.As<SkautSisCoreSettingsPart>();
            
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }
        
        [HttpGet, AlwaysAccessible]
        public ActionResult LogOn(string ReturnUrl) 
        {
            if (this.authenticationService.GetAuthenticatedUser() != null)
            {
                return this.RedirectLocal(ReturnUrl);
            }
            
            var skautIsUrl = this.coreSettings.SkautIsUrl;
            var appId = this.coreSettings.AppId;

            if (ReturnUrl != null)
            {
                var returnUri = new Uri(ReturnUrl);

                if (!returnUri.IsAbsoluteUri)
                {
                    return Redirect(String.Format("{0}Login/?appid={1}&ReturnURL={2}", skautIsUrl, appId, returnUri));
                }
            }

            return Redirect(String.Format("{0}Login/?appid={1}", skautIsUrl, appId));
        }

        [HttpPost, ActionName("LogOn"), AlwaysAccessible]
        public ActionResult LogOnPOST(bool? skautIS_Logout, Guid? skautIS_Token, int? skautIS_IDUnit, int? skautIS_IDRole, string ReturnURL) 
        {
            if (skautIS_Logout.HasValue && skautIS_Logout.Value)
            {
                // User was redirected from SkautIS after he has been logged out
                return this.RedirectLocal(ReturnURL);
            }
            else
            {
                // User was redirected from SkautIS after he has been successfully logged in
                if (!skautIS_Token.HasValue || !skautIS_IDRole.HasValue || !skautIS_IDUnit.HasValue)
                {
                    this.notifier.Error(T("Logon was unsuccessful."));
                    Logger.Debug("SkautIS login POST request detected, but not all required parameters were supplied.");
                }
                else
                {
                    var user = this.userService.GetOrCreateUser(skautIS_Token.Value);
                    var userPart = user.As<SkautIsUserPart>();
                    userPart.RoleId = skautIS_IDRole.Value;
                    userPart.UnitId = skautIS_IDUnit.Value;

                    if (user == null)
                    {
                        this.notifier.Error(T("Logon was unsuccessful."));
                        Logger.Debug(string.Format("Could not get/create user with token '{0}'", skautIS_Token.Value));
                    }
                    else
                    {
                        authenticationService.SignIn(user, false);
                    }
                }
                
                return this.RedirectLocal(ReturnURL);
            }
        }

        public ActionResult LogOff(string ReturnUrl, bool? tokenExpired)
        {
            var user = this.authenticationService.GetAuthenticatedUser();

            if (user != null)
            {
                var userPart = user.As<SkautIsUserPart>();

                if (userPart != null && userPart.Token.HasValue)
                {
                    var skautIsUrl = this.coreSettings.SkautIsUrl;
                    var appId = this.coreSettings.AppId;
                    var userToken = userPart.Token.Value;

                    this.authenticationService.SignOut();

                    if (tokenExpired.HasValue && tokenExpired.Value)
                    {
                        // User was redirected by Orchard after an expired token had been detected
                        this.notifier.Information(T("You have been logged out due to inactivity."));
                        return this.RedirectLocal(ReturnUrl);
                    }
                    else
                    {
                        if (ReturnUrl != null)
                        {
                            var returnUri = new Uri(ReturnUrl);

                            if (!returnUri.IsAbsoluteUri)
                            {
                                return Redirect(String.Format("{0}Login/LogOut.aspx?AppID={1}&Token={2}&ReturnURL={3}", skautIsUrl, appId, userToken, returnUri));
                            }
                        }

                        return Redirect(String.Format("{0}Login/LogOut.aspx?AppID={1}&Token={2}", skautIsUrl, appId, userToken));
                    }
                }
                else
                {
                    this.authenticationService.SignOut();
                }
            }

            return this.RedirectLocal(ReturnUrl);
        }
    }
}