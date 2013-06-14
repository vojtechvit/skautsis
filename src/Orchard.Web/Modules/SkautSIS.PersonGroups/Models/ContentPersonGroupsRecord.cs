using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace SkautSIS.PersonGroups.Models
{
    public class ContentPersonGroupsRecord
    {
        public virtual int Id { get; set; }
        public virtual PersonGroupsVisibilityPartRecord PersonGroupsVisibilityPartRecord { get; set; }
        public virtual int PersonGroupId { get; set; }
    }
}