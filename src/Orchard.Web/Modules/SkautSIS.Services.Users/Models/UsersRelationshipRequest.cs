using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkautSIS.Services.Users.Models
{
    public class UsersRelationshipRequest
    {
        [Key]
        public int Id { get; set; }
        public User Parent { get; set; }

        public string ChildFirstName { get; set; }
        public string ChildLastNmae { get; set; }
        public DateTime ChildBirthdate { get; set; }

        public StateWrapper State { get; set; }
    }
}