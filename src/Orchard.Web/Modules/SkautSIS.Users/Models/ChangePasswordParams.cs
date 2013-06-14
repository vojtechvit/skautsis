using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkautSIS.Users.Models
{
    public class ChangePasswordParams
    {
        public int SkautIsUserId { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordActual { get; set; }
    }
}