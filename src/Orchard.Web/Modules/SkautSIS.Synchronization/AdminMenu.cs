using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace SkautSIS.Synchronization
{
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
            menu.Add(T("Synchronization"),
                item => item.Action("Index", "SyncSettingsAdmin", new { area = "SkautSIS.Synchronization" }));

        }
    }
}