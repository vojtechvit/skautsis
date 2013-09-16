using Orchard;
using Orchard.ContentManagement;
using Orchard.Roles.Models;
using Orchard.Roles.Services;
using Orchard.Security;
using SkautSIS.Users.Extensions;
using SkautSIS.Users.Models;
using SkautSIS.Users.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SkautSIS.Users.Events
{
    public class SkautIsUserAuthorizationServiceEvenetHandler : IAuthorizationServiceEventHandler
    {
        public const string TokenRefreshedCacheKey = "SkautSIS.Users.TokenRefreshed";
        public const string RolesAssignedCacheKey = "SkautSIS.Users.RolesAssigned";

        private readonly IWorkContextAccessor workContextAccessor;
        private readonly ISkautIsUserService userService;
        private readonly IRoleService roleService;

        public SkautIsUserAuthorizationServiceEvenetHandler(
            IWorkContextAccessor workContextAccessor,
            ISkautIsUserService userService,
            IRoleService roleService)
        {
            this.workContextAccessor = workContextAccessor;
            this.userService = userService;
            this.roleService = roleService;
        }

        public void Checking(CheckAccessContext context)
        {
            if (context.User != null && this.userService.IsSkautIsUser(context.User))
            {
                var userPart = context.User.As<SkautIsUserPart>();

                RefreshUserToken(userPart);
                AddRoles(userPart);
            }
        }

        public void Adjust(CheckAccessContext context)
        { }

        public void Complete(CheckAccessContext context)
        { }

        private void RefreshUserToken(SkautIsUserPart part)
        {
            var workContext = this.workContextAccessor.GetContext();
            bool? tokenRefreshed = workContext.GetState<bool?>(TokenRefreshedCacheKey);

            if (!tokenRefreshed.HasValue || !tokenRefreshed.Value)
            {
                var user = workContext.CurrentUser;

                if (this.userService.IsSkautIsUser(user))
                {
                    if (this.userService.HasValidToken(user))
                    {
                        this.userService.RefreshToken(user);

                        workContext.SetState(TokenRefreshedCacheKey, (bool?)true);
                    }
                    else
                    {
                        // Redirect to logoff page
                        var request = workContext.HttpContext.Request;
                        var response = workContext.HttpContext.Response;

                        var urlHelper = new UrlHelper(request.RequestContext);
                        response.Redirect(urlHelper.LogOff(request.RawUrl, tokenExpired: true), true);
                    }
                }
            }
        }

        private void AddRoles(SkautIsUserPart userPart)
        {
            var workContext = this.workContextAccessor.GetContext();
            bool? rolesAssigned = workContext.GetState<bool?>(RolesAssignedCacheKey);

            if (!rolesAssigned.HasValue || !rolesAssigned.Value)
            {
                if (userPart.Has<IUserRoles>())
                {
                    var rolesPart = userPart.As<IUserRoles>();

                    var rolesToAssign = this.GetRolesAssignedByMembership(userPart)
                        .Union(this.GetRolesAssignedBySkautIsRoles(userPart))
                        .Distinct()
                        .ToArray();

                    foreach (var role in rolesToAssign)
                    {
                        if (!rolesPart.Roles.Contains(role))
                        {
                            rolesPart.Roles.Add(role);
                        }
                    }
                }

                // Cache assigned roles
                workContext.SetState(RolesAssignedCacheKey, (bool?)true);
            }
        }

        private IEnumerable<string> GetRolesAssignedByMembership(SkautIsUserPart userPart)
        {
            var usersSettings = workContextAccessor.GetContext().CurrentSite.As<SkautSisUsersSettingsPart>();

            return !String.IsNullOrEmpty(usersSettings.RolesAssignedByMembership)
                && userPart.HasMembership.HasValue 
                && userPart.HasMembership.Value
                ? usersSettings.RolesAssignedByMembership.Split(',')
                : new string[] { };
        }

        private IEnumerable<string> GetRolesAssignedBySkautIsRoles(SkautIsUserPart userPart)
        {
            var extCoreSettings = workContextAccessor.GetContext().CurrentSite.As<SkautSisCoreExtSettingsPart>();
            var usersSettings = workContextAccessor.GetContext().CurrentSite.As<SkautSisUsersSettingsPart>();

            var roles = new HashSet<string>();

            if (!String.IsNullOrEmpty(usersSettings.RolesAssignedBySkautIsRoles) && extCoreSettings.UnitId.HasValue)
            {
                // Dictionary: {UnitId} => {{RoleId},{RoleId},{RoleId}}
                var userSkautIsRoles = userPart.SkautIsRoles.Split(',').Select(x => x.Split(':')).ToDictionary(x => int.Parse(x.First()), x => x.Skip(1).Select(y => int.Parse(y)));

                if (userSkautIsRoles.ContainsKey(extCoreSettings.UnitId.Value))
                {
                    var roleAssignments = usersSettings.RolesAssignedBySkautIsRoles.Split(',').Select(x => x.Split(':')).ToDictionary(x => int.Parse(x.First()), x => x.Skip(1));

                    foreach (var roleId in userSkautIsRoles[extCoreSettings.UnitId.Value])
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