using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SkautSIS.Users.Models
{
    public class SkautIsUserParams
    {
        public int SkautIsUserId { get; set; }
        public int SkautIsPersonId { get; set; }

        public string UserName { get; set; }
        public int? SkautIsRoleId { get; set; }
        public int? SkautIsUnitId { get; set; }
        public Guid? SkautIsToken { get; set; }
        public DateTime? SkautIsTokenExpiration { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
    }
}