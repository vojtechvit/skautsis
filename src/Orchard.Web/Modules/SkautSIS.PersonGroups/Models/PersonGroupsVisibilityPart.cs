using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace SkautSIS.PersonGroups.Models
{
    public class PersonGroupsVisibilityPart : ContentPart<PersonGroupsVisibilityPartRecord>
    {
        public IEnumerable<int> PersonGroupIds 
        {
            get { return Record.PersonGroups.Select(p => p.PersonGroupId); }
        }
    }
}