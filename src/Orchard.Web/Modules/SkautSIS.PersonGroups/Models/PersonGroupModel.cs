using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using SkautSIS.PersonGroups.PersonGroupsModel;

namespace SkautSIS.PersonGroups.Models
{
    public class PersonGroupModel : IPersonGroup
    {
        public int PersonGroupId { get; set; }

        public string DisplayName { get; set; }
        
        public TypeEnum Type { get; set; }

        public ICollection<int> MemberSkautIsIds { get; set; }
    }
}