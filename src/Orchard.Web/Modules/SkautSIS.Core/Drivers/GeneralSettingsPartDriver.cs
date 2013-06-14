using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.UI.Notify;
using SkautSIS.Core.Models;

namespace SkautSIS.Core.Drivers
{
    public class GeneralSettingsPartDriver : ContentPartDriver<GeneralSettingsPart>
    {
        public Localizer T { get; set; }
        private readonly INotifier _notifier;

        public GeneralSettingsPartDriver(
            INotifier notifier) 
        {
            _notifier = notifier;
            T = NullLocalizer.Instance;
        }

        protected override DriverResult Editor(GeneralSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Share_Settings", 
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/Share.Settings",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(GeneralSettingsPart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null)) 
            {
                _notifier.Information(T("General settings updated successfully!"));
            }
            else 
            {
                _notifier.Error(T("Error during general settings update!"));   
            }

            return Editor(part, shapeHelper);
        }
    }
}