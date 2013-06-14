using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkautSIS.PersonGroups.Models;

namespace SkautSIS.PersonGroups.ViewModels
{
    public class EditPersonGroupsVisibilityViewModel
    {
        public IList<PersonGroupEntry> PersonGroups { get; set; }
    }
}