using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.UI.Notify;
using SkautSIS.Core.Services;
using SkautSIS.Synchronization.Models;
using SkautSIS.Synchronization.Services;
using SkautSIS.Users.Models;
using SkautSIS.Users.SkautIS_OU;
using SkautSIS.Users.SkautIS_UM;

namespace SkautSIS.Users.Services
{
    public class UsersSynchronizationService : ISynchronizationService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly Localizer T;
        private readonly ISkautSisServices _skautSisServices;
        private readonly IUserCentralizationService _userCentralizationService;

        private readonly UserManagementSoapClient _userManagement;
        private readonly OrganizationUnitSoap _organizationUnit;

        public UsersSynchronizationService(
            IOrchardServices orchardServices,
            IWorkContextAccessor workContextAccessor,
            ISkautSisServices skautSisServices,
            IUserCentralizationService userCentralizationService) 
        {
            _orchardServices = orchardServices;
            _skautSisServices = skautSisServices;
            _userCentralizationService = userCentralizationService;
            _workContextAccessor = workContextAccessor;
            T = NullLocalizer.Instance;

            _userManagement = new UserManagementSoapClient();
            _organizationUnit = new OrganizationUnitSoapClient();
        }


        public bool Synchronize() {
           try {
               // TODO authorizace vuci SkautISu
               var userToken = _workContextAccessor.GetContext().CurrentUser.As<SkautIsUserPart>().Record.SkautIsToken;
               var unitId = _skautSisServices.SkautSisSettings.UnitId;

               var unitAll = _organizationUnit.UnitTreeAll(new UnitTreeAllRequest(new UnitTreeAllRequestBody(new UnitTreeAllInput {
                   ID_Login = userToken,
                   ID_UnitParent = unitId,
                   ID_Application = _skautSisServices.SkautSisSettings.AppId
               })));

               foreach (var unit in unitAll.Body.UnitTreeAllResult) {
                   if (!unit.ID.HasValue) continue;


                   var personAll = _organizationUnit.PersonAll(new PersonAllRequest(new PersonAllRequestBody(new PersonAllInput {
                       ID_Login = userToken.Value,
                       ID_Unit = unit.ID_Unit
                   })));

                   foreach (var personOne in personAll.Body.PersonAllResult) {
                       if (personOne.ID != null) {
                           var person = _organizationUnit.PersonDetail(new PersonDetailRequest {
                               Body = new PersonDetailRequestBody(new PersonDetailInput {
                                   ID_Login = userToken.Value,
                                   ID = personOne.ID.Value
                               })
                           });
                           var p = person.Body.PersonDetailResult;

                           var personContacts = _organizationUnit.PersonContactAll(new PersonContactAllRequest(new PersonContactAllRequestBody(new PersonContactAllInput {
                               ID_Person = personOne.ID.Value,
                               ID_Login = userToken.Value,
                               ID_ContactType = "email_hlavni"
                           })));

                           //if (!p.ID_User.HasValue) continue;

                           var email = string.Empty;
                           if (personContacts.Body.PersonContactAllResult.Count() > 0) {
                               email = personContacts.Body.PersonContactAllResult[0].Value;
                           }

                           var userParams = new SkautIsUserCentralizedParams {
                               SkautIsPersonId = personOne.ID.Value,
                               FirstName = p.FirstName,
                               LastName = p.LastName,
                               NickName = p.NickName,
                               Email = email
                           };

                           if (_userCentralizationService.VerifyUnicity(personOne.ID.Value)) {
                               _userCentralizationService.CreateUser(userParams);
                           }
                           else {
                               _userCentralizationService.UpdateUser(userParams);
                           }
                       }
                   }
               }
           }
           catch (Exception e)
           {
               _orchardServices.Notifier.Error(T("An error occured during users synchronization with SkautIS: {0}", e.Message));
               return false;
           }

           _orchardServices.Notifier.Add(NotifyType.Information, T("Users synchronization succeeded."));
           return true;
        }

        public void SynchronizeUser(int skautIsUserId)
            {
            }
    }
}