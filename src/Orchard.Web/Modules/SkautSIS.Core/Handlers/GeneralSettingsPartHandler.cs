using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using SkautSIS.Core.Models;

namespace SkautSIS.Core.Handlers
{
    public class GeneralSettingsPartHandler : ContentHandler
    {
        public GeneralSettingsPartHandler(IRepository<GeneralSettingsPartRecord> repository) 
        {
            Filters.Add(new ActivatingFilter<GeneralSettingsPart>("SkautSisSettings"));
            Filters.Add(StorageFilter.For(repository));
        }
    }
}