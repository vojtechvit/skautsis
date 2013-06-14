using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace SkautSIS.PersonGroups {
    public class Permissions : IPermissionProvider {
        public static readonly Permission ManagePersonGroups = new Permission { Description = "Manage person groups", Name = "ManagePersonGroups" };
        public static readonly Permission ManagePersonGroupTypes = new Permission { Description = "Manage person group types", Name = "ManagePersonGroupTypes" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                ManagePersonGroups,
                ManagePersonGroupTypes
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {ManagePersonGroups,ManagePersonGroupTypes}
                },
            };
        }
    }
}
