using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using SkautSIS.Synchronization.Models;

namespace SkautSIS.Synchronization.Handlers
{
    public class SyncSettingsPartHandler : ContentHandler
    {
        public SyncSettingsPartHandler(IRepository<SyncSettingsPartRecord> repository) 
        {
            Filters.Add(new ActivatingFilter<SyncSettingsPart>("SyncSettings"));
            Filters.Add(StorageFilter.For(repository));
        }
    }
}