using SkautSIS.Users.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SkautSIS.Users.ViewModels
{
    public class EditSkautSisUsersSettingsViewModel
    {
        public bool SkautIsUserNeeded;

        public SkautSisUsersSettingsPart SettingsPart { get; set; }

        public IEnumerable<SelectListItem> SkautIsRoles { get; set; }

        public IEnumerable<SelectListItem> LocalRoles { get; set; }
    }
}