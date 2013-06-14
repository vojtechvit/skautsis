using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using SkautSIS.Users.Models;
using SkautSIS.Users.UsersModel;
namespace SkautSIS.Users.Services
{
    public interface IUserCentralizationService : IDependency 
    {
        SkautIsUserCentralizedParams GetUserCentralizedData(int skautIsUserId);
        IEnumerable<SkautIsUserCentralizedParams> GetUserAll();
        void CreateUser(SkautIsUserCentralizedParams userParams);
        void UpdateUser(SkautIsUserCentralizedParams userParams);
        void DeleteUser(int skautIsUserId);
        bool VerifyUnicity(int skautIsUserId);
    }
}