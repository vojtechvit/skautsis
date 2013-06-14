using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SkautSIS.Services.Core.Models;

namespace SkautSIS.Services.Users.Models
{
    public class UsersServiceRegistration : IServiceRegistration
    {
        public string Name {
            get { return "UsersService"; }
        }

        public string VirtualPath {
            get { return ConfigurationManager.AppSettings["UsersServiceVirtualPath"].ToString(); }
        }
    }
}