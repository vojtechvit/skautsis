using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.UI.Notify;
using SkautSIS.Synchronization.Models;

namespace SkautSIS.Synchronization.Drivers
{
    public class SyncSettingsPartDriver : ContentPartDriver<SyncSettingsPart>
    {
        public Localizer T { get; set; }
        private readonly INotifier _notifier;

        public SyncSettingsPartDriver(
            INotifier notifier) 
        {
            _notifier = notifier;
            T = NullLocalizer.Instance;
        }

        protected override DriverResult Editor(SyncSettingsPart part, dynamic shapeHelper)
        {
            part.AvailableFrequencyType = Enum.GetValues(typeof(Frequency)).Cast<int>().Select(i =>
                new
                {
                    Text = Enum.GetName(typeof(Frequency), i),
                    Value = i
                });

            return ContentShape("Parts_Share_SyncSettings", 
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/Share.SyncSettings",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(SyncSettingsPart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper)
        {
            
            if (!updater.TryUpdateModel(part, Prefix, null, null)) 
            {
                _notifier.Error(T("Error during synchronization settings update!"));   
            }

            return Editor(part, shapeHelper);
        }
    }
}