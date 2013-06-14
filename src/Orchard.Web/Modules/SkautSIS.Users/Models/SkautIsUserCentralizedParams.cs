using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkautSIS.Users.Models
{
    public class SkautIsUserCentralizedParams
    {
        public int SkautIsPersonId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }

        public string Email { get; set; }
    }
}