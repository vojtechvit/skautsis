using System;
using JetBrains.Annotations;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace SkautSIS.Core {

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
            menu.Add(T("Settings"),
                item => item.Action("Index", "SkautSisSettingsAdmin", new { area = "SkautSIS.Core" }));

        }
    }
}