using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace SkautSIS.Core.Models
{
    public class GeneralSettingsPart : ContentPart<GeneralSettingsPartRecord>, ISettings
    {
        public int UnitId 
        {
            get { return Record.UnitId; }
            set { Record.UnitId = value; }
        }

        public Guid AppId
        {
            get { return Record.AppId; }
            set { Record.AppId = value; }
            
        }

        public string SkautIsServicesUrl
        {
            get { return Record.SkautIsServicesUrl; }
            set { Record.SkautIsServicesUrl = value; }
        }

        public string SkautSisServicesUrl
        {
            get { return Record.SkautSisServicesUrl; }
            set { Record.SkautSisServicesUrl = value; }
        }
    }
}