using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Users.Models;
using SkautSIS.Core.Models;
using SkautSIS.Users.Models;
using SkautSIS.Users.Services.SkautIs.OrganizationUnit;
using SkautSIS.Users.Services.SkautIs.UserManagement;
using System;
using System.Linq;

namespace SkautSIS.Users.Services
{
    public class SkautIsUserService : ISkautIsUserService
    {
        public const int TokenExpirationPeriod = 30;

        private readonly IMembershipService membershipService;
        private readonly IAuthenticationService authenticationService;
        private readonly IPasswordGeneratorService passwordGeneratorService;
        private readonly Lazy<SkautSisCoreSettingsPart> coreSettings;
        private readonly IContentManager contentManager;
        private readonly IUsernameService usernameService;
        private readonly Lazy<UserManagementSoapClient> userManagementClient;
        private readonly Lazy<OrganizationUnitSoapClient> organizationUnitClient;
        
        public SkautIsUserService(
            IMembershipService membershipService,
            IAuthenticationService authenticationService,
            IPasswordGeneratorService passwordGeneratorService,
            IWorkContextAccessor workContextAccessor,
            IContentManager contentManager,
            IUsernameService usernameService)
        {
            this.membershipService = membershipService;
            this.authenticationService = authenticationService;
            this.passwordGeneratorService = passwordGeneratorService;
            this.contentManager = contentManager;
            this.usernameService = usernameService;

            this.coreSettings = new Lazy<SkautSisCoreSettingsPart>(
                () => workContextAccessor.GetContext().CurrentSite.As<SkautSisCoreSettingsPart>());

            this.userManagementClient = new Lazy<UserManagementSoapClient>(
                () => new UserManagementSoapClient(
                    this.coreSettings.Value.SkautIsWsSoapBinding,
                    this.coreSettings.Value.SkautIsUserManagementServiceAddress));

            this.organizationUnitClient = new Lazy<OrganizationUnitSoapClient>(
                () => new OrganizationUnitSoapClient(
                    this.coreSettings.Value.SkautIsWsSoapBinding,
                    this.coreSettings.Value.SkautIsOrganizationUnitServiceAddress));

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public bool IsSkautIsUser()
        {
            return this.IsSkautIsUser(null);
        }

        public bool IsSkautIsUser(IUser user)
        {
            user = user ?? this.authenticationService.GetAuthenticatedUser();

            if (user == null)
            {
                return false;
            }

            var userPart = user.As<SkautIsUserPart>();

            return userPart != null && userPart.SkautIsUserId.HasValue;
        }

        public void RefreshToken()
        {
            this.RefreshToken(null);
        }

        public void RefreshToken(IUser user)
        {
            user = user ?? this.authenticationService.GetAuthenticatedUser();
            var userPart = user.As<SkautIsUserPart>();

            if (this.IsSkautIsUser(user) && userPart.Token.HasValue)
            {
                var request = new LoginUpdateRefreshInput
                {
                    ID = userPart.Token.Value
                };

                try
                {
                    this.userManagementClient.Value.BeginLoginUpdateRefresh(request, null, null);
                    userPart.TokenExpiration = DateTime.UtcNow.AddMinutes(TokenExpirationPeriod);
                }
                catch (Exception e)
                {
                    Logger.Error(e, e.Message);
                }
            }
        }

        public bool HasValidToken()
        {
            return this.HasValidToken(null);
        }

        public bool HasValidToken(IUser user)
        {
            user = user ?? this.authenticationService.GetAuthenticatedUser();

            if (this.IsSkautIsUser(user))
            {
                var userPart = user.As<SkautIsUserPart>();

                return userPart.TokenExpiration.HasValue && userPart.TokenExpiration.Value > DateTime.UtcNow;
            }
            else
            {
                return true;
            }
        }

        public void InvalidateLoginData()
        {
            this.InvalidateLoginData(null);
        }

        public void InvalidateLoginData(IUser user)
        {
            user = user ?? this.authenticationService.GetAuthenticatedUser();

            if (this.IsSkautIsUser(user))
            {
                var userPart = user.As<SkautIsUserPart>();

                userPart.Token = null;
                userPart.TokenExpiration = null;
                userPart.RoleId = null;
                userPart.UnitId = null;
                userPart.HasMembership = null;
                userPart.SkautIsRoles = null;
            }
        }
        
        public void UpdateUserInfo()
        {
            this.UpdateUserInfo(null);
        }

        public void UpdateUserInfo(IUser user)
        {
            var currentUser = this.authenticationService.GetAuthenticatedUser();

            if (currentUser != null)
            {
                var currentUserSkautIsPart = currentUser.As<SkautIsUserPart>();

                if (this.IsSkautIsUser(currentUser) && currentUserSkautIsPart.Token.HasValue)
                {
                    this.UpdateUserInfo(user, currentUserSkautIsPart.Token.Value);
                }
            }
            else
            {
                Logger.Error("SkautIS user info update was called without a token, but no user is logged in.");
            }
        }

        public void UpdateUserInfo(IUser user, Guid token)
        {
            user = user ?? this.authenticationService.GetAuthenticatedUser();

            var userPart = user.As<UserPart>();
            var userSkautIsPart = user.As<SkautIsUserPart>();

            if (this.IsSkautIsUser(user))
            {
                int userSkautIsId = userSkautIsPart.SkautIsUserId.Value;
                
                try
                {
                    // Load personal data
                    var personDetail = this.organizationUnitClient.Value.PersonDetail(new PersonDetailInput
                    {
                        ID_Login = token,
                        ID = userSkautIsId
                    });

                    if (personDetail != null)
                    {
                        userPart.Email = personDetail.Email;
                        userSkautIsPart.FirstName = personDetail.FirstName;
                        userSkautIsPart.LastName = personDetail.LastName;
                        userSkautIsPart.NickName = personDetail.NickName;
                        userSkautIsPart.BirthDate = personDetail.Birthday;
                    }

                    // Load membership status
                    var userDetail = this.userManagementClient.Value.UserDetail(new UserDetailInput
                    {
                        ID_Login = token,
                        ID = userSkautIsId
                    });

                    userSkautIsPart.HasMembership = userDetail.HasMembership.Value;

                    // Load user's SkautIS roles
                    var userSkautIsRoles = this.userManagementClient.Value.UserRoleAll(new UserRoleAllInput
                    {
                        ID_Login = token,
                        ID_User = userSkautIsId,
                        IsActive = true
                    });

                    userSkautIsPart.SkautIsRoles = String.Join(",", userSkautIsRoles.Where(r => r.ID_Unit.HasValue).GroupBy(r => r.ID_Unit.Value).Select(g => g.Key + ":" + String.Join(":", g.Select(r => r.ID_Role.Value))));
                }
                catch (Exception e)
                {
                    Logger.Debug(e, e.Message);
                }
            }
        }

        public IUser GetOrCreateUser(int skautIsUserId)
        {
            var currentUser = this.authenticationService.GetAuthenticatedUser();

            if (currentUser != null)
            {
                var currentUserSkautIsPart = currentUser.As<SkautIsUserPart>();

                if (this.IsSkautIsUser(currentUser) && currentUserSkautIsPart.Token.HasValue)
                {
                    return this.GetOrCreateUser(skautIsUserId, currentUserSkautIsPart.Token.Value);
                }
                else
                {
                    Logger.Warning(string.Format("Attemp to get or create user from a SkautIS account (id: {0}) with current user's"
                        + " SkautIS token, but the current user doesn't have one.", skautIsUserId));

                    return null;
                }
            }
            else
            {
                Logger.Error("Attemp to get or create user from a SkautIS account (id: {0}) with current user's token, but no user is logged in.");
                return null;
            }
        }

        public IUser GetOrCreateUser(int skautIsUserId, Guid token)
        {
            try
            {
                var userDetail = this.userManagementClient.Value.UserDetail(new UserDetailInput
                {
                    ID = skautIsUserId,
                    ID_Login = token
                });

                return this.GetOrCreateUser(userDetail, token);
            }
            catch (Exception e)
            {
                Logger.Error(e, e.Message);
                return null;
            }
        }

        public IUser GetOrCreateUser(Guid userToken)
        {
            try
            {
                var userDetail = this.userManagementClient.Value.UserDetail(new UserDetailInput
                {
                    ID_Login = userToken
                });

                return this.GetOrCreateUser(userDetail, userToken);
            }
            catch (Exception e)
            {
                Logger.Error(e, e.Message);
                return null;
            }
        }

        private IUser GetOrCreateUser(UserDetailOutput userDetail, Guid token)
        {
            if (userDetail == null || !userDetail.ID.HasValue)
            {
                return null;
            }

            var userPart = this.contentManager.Query<SkautIsUserPart>()
                .Where<SkautIsUserPartRecord>(u => u.SkautIsUserId == userDetail.ID.Value)
                .List().FirstOrDefault();

            var user = (userPart != null)
                ? this.membershipService.GetUser(userPart.As<UserPart>().UserName)
                : this.CreateUser(userDetail, token);

            this.UpdateUserInfo(user);

            return user;
        }

        private IUser CreateUser(UserDetailOutput userDetail, Guid token)
        {
            var personDetail = this.organizationUnitClient.Value.PersonDetail(new PersonDetailInput
            {
                ID = userDetail.ID.Value,
                ID_Login = token
            });

            string uniqueUserName = usernameService.Calculate(userDetail.UserName);

            var newUser = this.membershipService.CreateUser(new CreateUserParams(
                uniqueUserName,
                passwordGeneratorService.Generate(),
                personDetail.Email,
                T("Auto Registered User").Text,
                passwordGeneratorService.Generate(),
                true));

            var userPart = newUser.As<SkautIsUserPart>();

            userPart.SkautIsUserId = userDetail.ID.Value;
            userPart.PersonId = personDetail.ID.Value;

            return newUser;
        }
    }
}