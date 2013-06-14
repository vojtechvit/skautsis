using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkautSIS.Services.Users.Models
{
    public class UsersRelationship
    {
        [Key]
        public int Id { get; set; }
        public User Parent { get; set; }
        public User Child { get; set; }
    }
}