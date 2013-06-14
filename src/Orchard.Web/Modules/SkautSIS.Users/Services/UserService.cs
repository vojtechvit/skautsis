using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;
using SkautSIS.Core.Models;
using SkautSIS.Users.Models;

namespace SkautSIS.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly ISkautIsMembershipService _membershipService;
        private readonly ISkautIsUserService _skautIsUserService;
        private readonly ISkautIsUserRoleService _skautIsUserRoleService;

        public UserService(
            IOrchardServices orchardServices,
            ISkautIsMembershipService membershipService,
            ISkautIsUserService skautIsUserService,
            ISkautIsUserRoleService skautIsUserRoleService)
        {
            _orchardServices = orchardServices;
            _skautIsUserService = skautIsUserService;
            _skautIsUserRoleService = skautIsUserRoleService;
            _membershipService = membershipService;
        }

        public IUser GetUser(Guid token, int? roleId, int? unitId)
        {
            //get SkautIS user ID
            var userId = _skautIsUserService.GetSkautIsUserId(token);
            if (userId == null) return null;

            var user = _membershipService.GetUser(userId.Value);
            // create new record
            if (user == null)
            {
                var userParams = _skautIsUserService.GetSkautIsUserParams(token);
                user = _membershipService.CreateUser(userParams);

                //if (_skautIsUserService.IsUnitAdmin(token, _orchardServices.WorkContext.CurrentSite.As<GeneralSettingsPart>().UnitId)) {
                    _skautIsUserRoleService.UserAddRole(user, "Administrator");
                //}
            }
            // update existing user
            else
            {
                // TODO update person records
                user.As<SkautIsUserPart>().Record.SkautIsToken = token;
                user.As<SkautIsUserPart>().Record.SkautIsTokenExpiration = DateTime.Now.AddMinutes(30);
            }

            return user;
        }
    }
}