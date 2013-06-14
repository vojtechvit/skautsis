using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkautSIS.Core.Models;

namespace SkautSIS.Core.ViewModels
{
    public class GeneralSettingsViewModel
    {
        public GeneralSettingsPart Settings { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id
        {
            get { return Settings.ContentItem.Id; }
        }

        public int UnitId
        {
            get { return Settings.Record.UnitId; }
            set { Settings.Record.UnitId = value; }
        }


        public Guid AppId
        {
            get { return Settings.Record.AppId; }
            set { Settings.Record.AppId = value; }
        }

        public string SkautIsServicesUrl
        {
            get { return Settings.Record.SkautIsServicesUrl; }
            set { Settings.Record.SkautIsServicesUrl = value; }
        }

        public string SkautSisServicesUrl
        {
            get { return Settings.Record.SkautSisServicesUrl; }
            set { Settings.Record.SkautSisServicesUrl = value; }
        }
    }
}