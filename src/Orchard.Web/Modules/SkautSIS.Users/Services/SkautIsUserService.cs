using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Security;
using SkautSIS.Users.Models;
using SkautSIS.Users.SkautIS_OU;
using SkautSIS.Users.SkautIS_UM;
using CreateUserParams = SkautSIS.Users.Models.CreateUserParams;

namespace SkautSIS.Users.Services
{
    public class SkautIsUserService : ISkautIsUserService
    {
        private readonly UserManagementSoapClient _userManagement;
        private readonly OrganizationUnitSoap _organizationUnit;
        
        public SkautIsUserService() 
        {
            _userManagement = new UserManagementSoapClient();
            _organizationUnit = new OrganizationUnitSoapClient();
        }


        public int? GetSkautIsUserId(Guid skautIsToken) 
        {
            var user = _userManagement.UserDetail(new UserDetailInput {ID_Login = skautIsToken});

            return user.ID;
        }

        public SkautIsUserParams GetSkautIsUserParams(Guid skautIsToken) 
        {
            var up = new SkautIsUserParams();
            
            var user = _userManagement.UserDetail(new UserDetailInput 
            {
                ID_Login = skautIsToken
            });

            int? personID = user.ID_Person;

            if (personID != null) 
            {
                var person = _organizationUnit.PersonDetail(new PersonDetailRequest 
                {
                    Body = new PersonDetailRequestBody(new PersonDetailInput 
                    {
                        ID_Login = skautIsToken,
                        ID = personID.Value
                    })
                });
                var p = person.Body.PersonDetailResult;

                up.FirstName = p.FirstName;
                up.LastName = p.LastName;
                up.NickName = p.NickName;

                var personContacts = _organizationUnit.PersonContactAll(new PersonContactAllRequest(new PersonContactAllRequestBody(new PersonContactAllInput 
                {
                    ID_Person = personID.Value,
                    ID_Login = skautIsToken,
                    ID_ContactType = "email_hlavni"
                })));

                up.Email = personContacts.Body.PersonContactAllResult[0].Value;
                up.SkautIsPersonId = personID.Value;
            }

            up.UserName = user.UserName;
            up.SkautIsToken = skautIsToken;
            up.SkautIsTokenExpiration = DateTime.Now.AddMinutes(30);
            up.SkautIsUserId = user.ID.Value;

            return up;
        }
        
        public bool IsUnitAdmin(Guid skautIsToken, int unitId) 
        {
            //var user = _userManagement.UserDetail(new UserDetailInput {
            //    ID_Login = skautIsToken
            //});

            ////var roleAll = _userManagement.RoleAll(new RoleAllInput {
            ////    ID_Login = skautIsToken
            ////});

            //var userRoleAll = _userManagement.UserRoleAll(new UserRoleAllInput {
            //    ID_Login = skautIsToken,
            //    ID_User = user.ID
            //});

            //var unitDetail = _organizationUnit.UnitDetail(new UnitDetailRequest(new UnitDetailRequestBody(new UnitDetailInput {
            //    ID_Login = skautIsToken, 
                //ID
            //})));

            return true;
        }

        public void LogOut(Guid skautIsToken) 
        {
            _userManagement.LoginUpdateLogout(
                new LoginUpdateLogoutInput 
                {
                    ID = skautIsToken
                });
        }

        public bool VerifyUserUnicity(string userName, string email) 
        {
            //TODO IS verification email unicity
            return true;
        }

        public int? CreateSkautIsUser(CreateUserParams userParams) {
            return _userManagement.UserInsert(new UserInsertInput {
                UserName = userParams.UserName,
                Email = userParams.Email,
                FirstName = userParams.FirstName,
                LastName = userParams.LastName,
                NickName = userParams.NickName,
                Password = userParams.Password
            }).ID;
        }

        public void ChangePassword(Guid skautIsToken, ChangePasswordParams passwordParams) 
        {
            _userManagement.UserUpdate(new UserUpdateInput {
                ID_Login = skautIsToken,
                ID = passwordParams.SkautIsUserId,
                Password = passwordParams.PasswordNew,
                PasswordActual = passwordParams.PasswordActual
            });
        }

        public int? GetSkautIsPersonId(Guid skautIsToken, int skautIsUserId) {
            return _userManagement.UserDetail(new UserDetailInput {
                ID_Login = skautIsToken,
                ID = skautIsUserId
            }).ID_Person;
        }
    }
}