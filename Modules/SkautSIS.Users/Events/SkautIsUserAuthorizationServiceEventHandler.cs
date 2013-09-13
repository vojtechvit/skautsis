using Orchard;
using Orchard.ContentManagement;
using Orchard.Roles.Models;
using Orchard.Roles.Services;
using Orchard.Security;
using SkautSIS.Core.Models;
using SkautSIS.Users.Models;
using SkautSIS.Users.Services;
using System.Collections.Generic;
using System.Linq;

namespace SkautSIS.Users.Events
{
    public class SkautIsUserAuthorizationServiceEvenetHandler : IAuthorizationServiceEventHandler
    {
        private const string AssignedRolesCacheKey = "SkautSIS.Users.AssignedRoles";
        private readonly ISkautIsUserService userService;
        private readonly IRoleService roleService;
        private readonly IWorkContextAccessor workContextAccessor;

        public SkautIsUserAuthorizationServiceEvenetHandler(
            ISkautIsUserService userService,
            IRoleService roleService,
            IWorkContextAccessor workContextAccessor)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.workContextAccessor = workContextAccessor;
        }

        public void Checking(CheckAccessContext context)
        {
            if (context.User != null && this.userService.IsSkautIsUser(context.User))
            {
                var userPart = context.User.As<SkautIsUserPart>();

                if (context.User.Has<IUserRoles>())
                {
                    var rolesPart = context.User.As<IUserRoles>();
                    var workContext = this.workContextAccessor.GetContext();

                    var rolesToAssign = workContext.GetState<IEnumerable<string>>(AssignedRolesCacheKey);

                    if (rolesToAssign == null)
                    {
                        rolesToAssign = this.GetRolesAssignedByMembership(userPart).Union(this.GetRolesAssignedBySkautIsRoles(userPart));
                        workContext.SetState(AssignedRolesCacheKey, rolesToAssign);
                    }
                    
                    foreach (var role in rolesToAssign)
                    {
                        if (!rolesPart.Roles.Contains(role))
                        {
                            rolesPart.Roles.Add(role);
                        }
                    }
                }
            }
        }

        public void Adjust(CheckAccessContext context)
        { }

        public void Complete(CheckAccessContext context)
        { }

        private IEnumerable<string> GetRolesAssignedByMembership(SkautIsUserPart userPart)
        {
            var usersSettings = workContextAccessor.GetContext().CurrentSite.As<SkautSisUsersSettingsPart>();

            return userPart.HasMembership.HasValue && userPart.HasMembership.Value
                ? usersSettings.RolesAssignedByMembership.Split(',')
                : new string[] { };
        }

        private IEnumerable<string> GetRolesAssignedBySkautIsRoles(SkautIsUserPart userPart)
        {
            var coreSettings = workContextAccessor.GetContext().CurrentSite.As<SkautSisCoreSettingsPart>();
            var usersSettings = workContextAccessor.GetContext().CurrentSite.As<SkautSisUsersSettingsPart>();

            var roles = new HashSet<string>();

            if (coreSettings.UnitId.HasValue)
            {
                // Dictionary: {UnitId} => {{RoleId},{RoleId},{RoleId}}
                var userSkautIsRoles = userPart.SkautIsRoles.Split(',').Select(x => x.Split(':')).ToDictionary(x => int.Parse(x.First()), x => x.Skip(1).Select(y => int.Parse(y)));

                if (userSkautIsRoles.ContainsKey(coreSettings.UnitId.Value))
                {
                    var roleAssignments = usersSettings.RolesAssignedBySkautIsRoles.Split(',').Select(x => x.Split(':')).ToDictionary(x => int.Parse(x.First()), x => x.Skip(1));

                    foreach (var roleId in userSkautIsRoles[coreSettings.UnitId.Value])
                    {
                        if (roleAssignments.ContainsKey(roleId))
                        {
                            foreach (var role in roleAssignments[roleId])
                            {
                                roles.Add(role);
                            }
                        }
                    }
                }
            }

            return roles;
        }
    }
}