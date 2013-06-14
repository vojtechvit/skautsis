using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SkautSIS.PersonGroups.Models;
using SkautSIS.Users.Models;

namespace SkautSIS.PersonGroups.ViewModels
{
    public class PersonGroupCreateViewModel
    {
        [Required, StringLength(255)]
        public string DisplayName { get; set; }
        public IList<PersonEntry> Persons { get; set; }
    }

    public class PersonEntry
    {
        public SkautIsUserCentralizedParams Person { get; set; }
        public bool IsChecked { get; set; }
    }
}