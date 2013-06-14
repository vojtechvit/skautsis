using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Localization;
using Orchard.Security;
using SkautSIS.Core.Services;
using SkautSIS.PersonGroups.Models;
using SkautSIS.PersonGroups.PersonGroupsModel;
using SkautSIS.Synchronization.Models;
using SkautSIS.Synchronization.Services;
using SkautSIS.Users.Models;
using Orchard.ContentManagement;
using SkautSIS.Users.SkautIS_OU;
using SkautSIS.Users.SkautIS_UM;
using Person = SkautSIS.PersonGroups.PersonGroupsModel.Person;

namespace SkautSIS.PersonGroups.Services
{
    public class PersonGroupsSynchronizationService : ISynchronizationService 
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly Localizer T;
        private readonly ISkautSisServices _skautSisServices;
        private readonly IPersonGroupsService _personGroupService;

        private readonly OrganizationUnitSoap _organizationUnit;
        private readonly UserManagementSoapClient _userManagement;

        public PersonGroupsSynchronizationService(
            IOrchardServices orchardServices,
            IWorkContextAccessor workContextAccessor,
            ISkautSisServices skautSisServices,
            IPersonGroupsService personGroupService) 
        {
            _orchardServices = orchardServices;
            _skautSisServices = skautSisServices;
            _personGroupService = personGroupService;
            _workContextAccessor = workContextAccessor;
            T = NullLocalizer.Instance;

            _userManagement = new UserManagementSoapClient();
            _organizationUnit = new OrganizationUnitSoapClient();
        }

        public bool Synchronize() 
        {
            // TODO authorizace vuci SkautISu
            //if (!_skautSisServices.SkautIsAuthorizer.Authorize(new List<string> { "unitAdmin", "subUnitAdmin" })) return false;

            var userToken = _workContextAccessor.GetContext().CurrentUser.As<SkautIsUserPart>().Record.SkautIsToken; 
            var unitId = _skautSisServices.SkautSisSettings.UnitId;

            var unitAll = _organizationUnit.UnitTreeAll(new UnitTreeAllRequest(new UnitTreeAllRequestBody(new UnitTreeAllInput {
                ID_Login = userToken,
                ID_UnitParent = unitId,
                ID_Application = _skautSisServices.SkautSisSettings.AppId
            })));

            foreach (var unit in unitAll.Body.UnitTreeAllResult) 
            {
                if (!unit.ID.HasValue) continue;
                var unitDetail = _organizationUnit.UnitDetail(new UnitDetailRequest(new UnitDetailRequestBody(new UnitDetailInput
                {
                    ID_Login = userToken,
                    ID = unit.ID_Unit.Value
                })));

                var personAll = _organizationUnit.PersonAll(new PersonAllRequest(new PersonAllRequestBody(new PersonAllInput {
                    ID_Login = userToken.Value,
                    ID_Unit = unit.ID_Unit
                })));

                var userIds = (from person in personAll.Body.PersonAllResult where person.ID.HasValue select person.ID.Value).ToList();

                foreach (var userId in userIds) {
                    _personGroupService.CreatePerson(userId);
                }

                var u = unitDetail.Body.UnitDetailResult;

                var newGroupName = "Členové " + u.ID + "-" + u.DisplayName;
                if(_personGroupService.VerifyUnicity(newGroupName)) 
                {
                    _personGroupService.Create(new PersonGroupModel {
                        DisplayName = newGroupName,
                        Type = TypeEnum.Unit,
                        MemberSkautIsIds = userIds
                    });
                } else {
                    var oldGroup = _personGroupService.GetSingle(newGroupName);
                    _personGroupService.UpdateMembers(oldGroup.PersonGroupId, userIds.Select(x=>x.ToString()));
                }
            }

            return true;
        }
    }
}