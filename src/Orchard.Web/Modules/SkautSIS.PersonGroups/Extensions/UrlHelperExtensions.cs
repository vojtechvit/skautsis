using System.Web.Mvc;

namespace SkautSIS.PersonGroups.Extensions 
{
    public static class UrlHelperExtensions
    {
        public static string PersonGroupCreate(this UrlHelper urlHelper)
        {
            return urlHelper.Action("Create", "PersonGroupsAdmin", new { area = "SkautSIS.PersonGroups" });
        }

        public static string PersonGroupList(this UrlHelper urlHelper)
        {
            return urlHelper.Action("Index", "PersonGroupsAdmin", new { area = "SkautSIS.PersonGroups" });
        }
    }
}