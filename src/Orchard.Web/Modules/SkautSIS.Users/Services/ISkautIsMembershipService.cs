using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Security;
using SkautSIS.Users.Models;

namespace SkautSIS.Users.Services
{
    public interface ISkautIsMembershipService : IMembershipService, IDependency 
    {
        IUser GetUser(int skautIsUserId);
        bool LocalUserExists(int skautIsUserId);
        IUser CreateUser(SkautIsUserParams userParams);
    }
}