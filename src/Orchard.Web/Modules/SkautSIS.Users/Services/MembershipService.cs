using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using SkautSIS.Users.Models;
using SkautSIS.Users.SkautIS_UM;

namespace SkautSIS.Users.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Orchard.Security;

    public class MembershipService : ISkautIsMembershipService
    {
        private readonly UserManagementSoapClient _skautIsUserService;
        private readonly IContentManager _contentManager;
        private readonly IUserCentralizationService _userCentralizationService;
        private readonly IOrchardServices _orchardServices;

        public Localizer T { get; set; }
        
        public MembershipService(
            IOrchardServices orchardServices,
            IUserCentralizationService userCentralizationService,
            IContentManager contentManager) 
        {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _userCentralizationService = userCentralizationService;
            _skautIsUserService = new UserManagementSoapClient();
            T = NullLocalizer.Instance;
        }

        public IUser GetUser(int skautIsUserId)
        {
            return _orchardServices.ContentManager.Query<SkautIsUserPart, SkautIsUserPartRecord>().Where(u => u.SkautIsUserId == skautIsUserId).List().FirstOrDefault();
        }

        public IUser CreateUser(SkautIsUserParams userParams)
        {
            var user = _orchardServices.ContentManager.New<SkautIsUserPart>("User");

            user.Record.SkautIsUserId = userParams.SkautIsUserId;
            user.Record.SkautIsPersonId = userParams.SkautIsPersonId;
            user.Record.UserName = userParams.UserName;
            user.Record.SkautIsToken = userParams.SkautIsToken;
            user.Record.SkautIsTokenExpiration = userParams.SkautIsTokenExpiration;
            user.Record.SkautIsUnitId = userParams.SkautIsUnitId;

            _userCentralizationService.CreateUser(new SkautIsUserCentralizedParams {
                Email = userParams.Email,
                FirstName = userParams.FirstName,
                LastName = userParams.LastName,
                NickName = userParams.NickName,
                SkautIsPersonId = userParams.SkautIsPersonId
            });

            _orchardServices.ContentManager.Create(user);

            return user;
        }

        public bool LocalUserExists(int skautIsUserId) 
        {
            return _orchardServices.ContentManager.Query<SkautIsUserPart, SkautIsUserPartRecord>()
                .Where(u => u.SkautIsUserId == skautIsUserId).List().FirstOrDefault() != null ? true : false;
        }


        public MembershipSettings GetSettings() {
            var settings = new MembershipSettings();
            return settings;
        }

        public IUser GetUser(string username) 
        {
            return _orchardServices.ContentManager.Query<SkautIsUserPart, SkautIsUserPartRecord>().Where(u => u.UserName == username).List().FirstOrDefault();
        }

        public IUser ValidateUser(string userNameOrEmail, string password) {
            throw new NotImplementedException();
        }

        public void SetPassword(IUser user, string password) {
            throw new NotImplementedException();
        }

        public IUser CreateUser(CreateUserParams createUserParams)
        {
            throw new NotImplementedException();
        }
    }
}