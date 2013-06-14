using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security.Permissions;

namespace SkautSIS.Synchronization
{
    public class Persmissions : IPermissionProvider {
        public static readonly Permission ManageSynchronizationSettings = new Permission { Description = "Manage configuration of the synchronization.", Name = "ManageSynchronizationSettings" };
        public static readonly Permission PerformSynchronization = new Permission { Description = "Perform system synchronization.", Name = "PerformSynchronization" };

        public Orchard.Environment.Extensions.Models.Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] {
                ManageSynchronizationSettings, 
                PerformSynchronization
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {
                        ManageSynchronizationSettings,
                        PerformSynchronization
                    }
                }
            };
        }
    }
}