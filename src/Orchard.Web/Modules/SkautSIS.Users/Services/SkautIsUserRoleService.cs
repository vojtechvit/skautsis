using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Roles.Models;
using Orchard.Roles.Services;
using Orchard.Security;
using SkautSIS.Synchronization.Models;
using SkautSIS.Users.Models;

namespace SkautSIS.Users.Services
{
    public class SkautIsUserRoleService : ISkautIsUserRoleService {
        private readonly IRoleService _roleService;
        private readonly IRepository<SkautIsUserPartRecord> _userPartRepository;
        private readonly IRepository<UserRolesPartRecord> _userRolesPartRepository;
        private readonly IOrchardServices _orchardServices;

        public SkautIsUserRoleService(
            IRoleService roleService,
            IOrchardServices orchardServices,
            IRepository<SkautIsUserPartRecord> userPartRepository,
            IRepository<UserRolesPartRecord> userRolesPartRepository) 
        {
            _orchardServices = orchardServices;
            _roleService = roleService;

            _userPartRepository = userPartRepository;
            _userRolesPartRepository = userRolesPartRepository;
        }

        public void UserAddRole(IUser user, string roleName) 
        {
            var role = _roleService.GetRoleByName(roleName);

            var userId = user.As<SkautIsUserPart>().Id;

            //if (_userRolesPartRepository.Fetch(u => u.UserId == userId & u.Role == role).Any()) return;

            var part = new UserRolesPartRecord {
                Role = role,
                UserId = userId
            };

            _userRolesPartRepository.Create(part);
        }
    }
}