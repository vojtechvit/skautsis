using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using SkautSIS.PersonGroups.Models;

namespace SkautSIS.PersonGroups.Handlers
{
    public class PersonGroupsVisibilityPartHandler : ContentHandler 
    {
        public  PersonGroupsVisibilityPartHandler(IRepository<PersonGroupsVisibilityPartRecord> repository) 
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}