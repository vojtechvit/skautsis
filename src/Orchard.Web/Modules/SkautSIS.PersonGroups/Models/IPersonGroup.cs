using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using SkautSIS.PersonGroups.PersonGroupsModel;

namespace SkautSIS.PersonGroups.Models
{
    public interface IPersonGroup : IDependency
    {
        int PersonGroupId { get; set; }
        string DisplayName { get; set; }
        TypeEnum Type { get; set; }
        ICollection<int> MemberSkautIsIds { get; set; }
    }
}