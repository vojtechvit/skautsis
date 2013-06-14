using System;
using JetBrains.Annotations;
using Orchard.Core.Contents;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace SkautSIS.Users {

    public class AdminMenu : INavigationProvider 
    {
        public Localizer T { get; set; }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("SkautSIS"), "1.0", BuildMenu);
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(T("Users"),
                item => item.Action("Index", "AccountAdmin", new { area = "SkautSIS.Users" }).Permission(Permissions.ManageUsers));
        }
    }
}