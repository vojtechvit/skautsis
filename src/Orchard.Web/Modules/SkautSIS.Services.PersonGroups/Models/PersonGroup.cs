using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Services;
using System.Linq;
using System.Web;

namespace SkautSIS.Services.PersonGroups.Models
{
    public class PersonGroup
    {
        [Key]
        public int PersonGroupId { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public int Type { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}