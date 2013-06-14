using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace SkautSIS.Users 
{
    public class Permissions : IPermissionProvider 
    {
        public static readonly Permission ManageUsers = new Permission { Description = "Manage users.", Name = "ManageUsers" };
        public static readonly Permission ManageUserRegistrationSettings = new Permission { Description = "Manage user registration setting.", Name = "ManageUserRegistrationSettings" };
        public static readonly Permission ManageUserRoles = new Permission { Description = "Manage user roles.", Name = "ManageUserRoles" };
        public static readonly Permission ViewUserDetail = new Permission { Description = "To see user detailed information.", Name = "ViewUserDetail" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                ManageUsers,
                ManageUserRoles,
                ViewUserDetail,
                ManageUserRegistrationSettings
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {
                        ManageUsers,
                        ManageUserRoles,
                        ViewUserDetail,
                        ManageUserRegistrationSettings
                    }
                },
            };
        }
    }
}
