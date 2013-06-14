using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.ContentManagement;
using SkautSIS.PersonGroups.Models;
using SkautSIS.PersonGroups.ViewModels;

namespace SkautSIS.PersonGroups.Services
{
    public interface IPersonGroupsVisibilityService : IDependency {
        void UpdatePersonGroupsForContentItem(
            ContentItem item,
            IEnumerable<PersonGroupEntry> personGroups);
    }
}