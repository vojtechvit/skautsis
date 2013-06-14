using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace SkautSIS.PersonGroups.Models
{
    public class PersonGroupsVisibilityPartRecord : ContentPartRecord
    {
        public PersonGroupsVisibilityPartRecord() 
        {
            PersonGroups = new List<ContentPersonGroupsRecord>();
        }

        public virtual IList<ContentPersonGroupsRecord> PersonGroups { get; set; }
    }
}