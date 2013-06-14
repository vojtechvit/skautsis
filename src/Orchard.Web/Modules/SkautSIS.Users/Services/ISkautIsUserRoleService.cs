using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Security;
using SkautSIS.Synchronization.Services;

namespace SkautSIS.Users.Services
{
    public interface ISkautIsUserRoleService : IDependency {
        void UserAddRole(IUser user, string roleName);
    }
}