namespace SkautSIS.Services.PersonGroups.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        
        [Required]
        public int SkautIsUserId { get; set; }

        public virtual ICollection<PersonGroup> Groups { get; set; }
    }
}