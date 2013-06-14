using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Security;
using SkautSIS.Users.Models;
using CreateUserParams = SkautSIS.Users.Models.CreateUserParams;

namespace SkautSIS.Users.Services
{
    public interface ISkautIsUserService : IDependency 
    {
        int? GetSkautIsUserId(Guid skautIsToken);
        bool IsUnitAdmin(Guid skautIsToken, int unitId);
        void LogOut(Guid skautIsToken);


        SkautIsUserParams GetSkautIsUserParams(Guid skautIsToken);
        //SkautIsUserParams GetSkautIsUserParams(Guid skautIsToken, int skautIsUserId);

        bool VerifyUserUnicity(string userName, string email);
        int? CreateSkautIsUser(CreateUserParams userParams);
        void ChangePassword(Guid skautIsToken, ChangePasswordParams passwordParams);
        int? GetSkautIsPersonId(Guid skautIsToken, int skautIsUserId);
    }
}