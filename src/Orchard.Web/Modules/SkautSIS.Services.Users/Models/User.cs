using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkautSIS.Services.Users.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SkautIsPersonId { get; set; }
        //public string UserName { get; set; }
        //public int UnitId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }

        public string Email { get; set; }
    }
}