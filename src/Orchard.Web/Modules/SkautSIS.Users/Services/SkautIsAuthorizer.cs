using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Localization;
using Orchard.UI.Notify;
using SkautSIS.Core.Services;
using SkautSIS.Users.Models;
using Orchard.ContentManagement;
using SkautSIS.Users.SkautIS_UM;

namespace SkautSIS.Users.Services
{
    public class SkautIsAuthorizer : ISkautIsAuthorizer
    {
        private readonly INotifier _notifier;
        private readonly IWorkContextAccessor _workContextAccessor;
        public Localizer T { get; set; }

        private readonly UserManagementSoapClient _userManagement;

        public SkautIsAuthorizer(
            INotifier notifier,
            IWorkContextAccessor workContextAccessor) 
        {
            _notifier = notifier;
            _workContextAccessor = workContextAccessor;
            T = NullLocalizer.Instance;

            _userManagement = new UserManagementSoapClient();
        }
        public bool Authorize(IList<string> skautIsRoles) 
        {
            return Authorize(skautIsRoles, null);
        }

        public bool Authorize(IList<string> skautIsRoles, LocalizedString message) 
        {
            //TODO Authozization
            var userToken = _workContextAccessor.GetContext().CurrentUser.As<SkautIsUserPart>().Record.SkautIsToken; 
            var userId = _workContextAccessor.GetContext().CurrentUser.As<SkautIsUserPart>().Record.SkautIsUserId; 

            if (userToken == null) {
                _notifier.Error(T("Invalid SkautIS login token. Try to re-sing in."));
            } else
            {
                var actualRoleId = _workContextAccessor.GetContext().CurrentUser.As<SkautIsUserPart>().Record.SkautIsRoleId;
                if (actualRoleId.HasValue) 
                {
                    var roleDetail = _userManagement.RoleDetail(new RoleDetailInput {
                        ID_Login = userToken.Value,
                        ID = actualRoleId.Value
                    });

                    if (skautIsRoles.Contains(roleDetail.DisplayName)) return true;
                }

                var rolesForUser = _userManagement.UserRoleAll(new UserRoleAllInput {
                    ID_Login = userToken.Value,
                    ID_User = userId
                });

                foreach (var role in rolesForUser) {
                    if (skautIsRoles.Contains(role.DisplayName)) return true;
                }
            }
            

            if (message != null)
            {
                if (_workContextAccessor.GetContext().CurrentUser == null)
                {
                    _notifier.Error(T("{0}. Anonymous users do not have permission for acces to SkautIS.",
                                      message));
                }
                else
                {
                    _notifier.Error(T("{0}. Current user, {1}, does not have permission for acces to SkautIS.",
                                      message, _workContextAccessor.GetContext().CurrentUser.UserName));
                }
            }

            return false;
        }
    }
}