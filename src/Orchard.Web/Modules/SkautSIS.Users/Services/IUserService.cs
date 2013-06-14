using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Security;

namespace SkautSIS.Users.Services
{
    public interface IUserService :IDependency {
        IUser GetUser(Guid token, int? roleId, int? unitId);
    }
}