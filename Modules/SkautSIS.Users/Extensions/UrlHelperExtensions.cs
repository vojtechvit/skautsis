using System.Web.Mvc;
using System.Web.Routing;

namespace SkautSIS.Users.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string LogOn(this UrlHelper urlHelper, string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return urlHelper.Action("LogOn", "Account", new { area = Constants.OrchardUsersArea, ReturnUrl = returnUrl });
            }

            return urlHelper.Action("LogOn", "Account", new { area = Constants.OrchardUsersArea });
        }

        public static string SkautIsLogOn(this UrlHelper urlHelper, string returnUrl)
        {
            return urlHelper.Action("LogOn", "Account", new { area = Constants.LocalArea, ReturnUrl = returnUrl });
        }

        public static string LogOff(this UrlHelper urlHelper, string returnUrl, bool tokenExpired = false)
        {
            var routeValues = new RouteValueDictionary
            {
                { "area", Constants.LocalArea }
            };

            if (tokenExpired)
            {
                routeValues.Add("tokenExpired", true);
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                routeValues.Add("ReturnUrl", returnUrl);
            }

            return urlHelper.Action("LogOff", "Account", routeValues);
        }

        public static string LocalUserManagement(this UrlHelper urlHelper)
        {
            return urlHelper.Action("ChangePassword", "Account", new { area = Constants.OrchardUsersArea });
        }

        public static string SkautIsUserManagement(this UrlHelper urlHelper)
        {
            return "https://is.skaut.cz/Junak/OrganizationUnit/PersonDetail.aspx";
        }

        public static string LocalRegister(this UrlHelper urlHelper)
        {
            return urlHelper.Action("Register", "Account", new { area = Constants.OrchardUsersArea });
        }

        public static string SkautIsRegister(this UrlHelper urlHelper)
        {
            return "https://is.skaut.cz/Login/Registration.aspx";
        }
    }
}