using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security.Permissions;

namespace SkautSIS.Core
{
    public class Persmissions : IPermissionProvider {
        public static readonly Permission ManageSkautSisSettings = new Permission { Description = "Manage SkautSIS configuration.", Name = "ManageSkautSisSettings" };

        public Orchard.Environment.Extensions.Models.Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] {
                ManageSkautSisSettings 
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {ManageSkautSisSettings}
                }
            };
        }
    }
}