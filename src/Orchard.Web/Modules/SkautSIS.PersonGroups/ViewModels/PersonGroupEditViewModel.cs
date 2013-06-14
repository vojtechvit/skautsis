using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkautSIS.PersonGroups.ViewModels
{
    public class PersonGroupEditViewModel
    {
        [Required, StringLength(255)]
        public string DisplayName { get; set; }
        [Required]
        public int PersonGroupId { get; set; }
        public IList<PersonEntry> Persons { get; set; }
    }
}