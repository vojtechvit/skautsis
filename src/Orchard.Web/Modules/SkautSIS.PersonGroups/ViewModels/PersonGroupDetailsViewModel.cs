using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkautSIS.PersonGroups.Models;
using SkautSIS.Users.Models;

namespace SkautSIS.PersonGroups.ViewModels
{
    public class PersonGroupDetailsViewModel
    {
        public IPersonGroup PersonGroup { get; set; }
        public ICollection<SkautIsUserCentralizedParams> Members { get; set; }
    }
}