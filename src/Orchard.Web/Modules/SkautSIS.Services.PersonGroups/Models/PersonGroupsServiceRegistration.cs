using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SkautSIS.Services.Core.Models;

namespace SkautSIS.Services.PersonGroups.Models
{
    public class PersonGroupsServiceRegistration : IServiceRegistration
    {
        public string Name {
            get { return "PersonGroupsService"; }
        }

        public string VirtualPath
        {
            get { return ConfigurationManager.AppSettings["PersonGroupsServiceVirtualPath"].ToString(); }
        }
    }
}