

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkautSIS.Core.Services;
using SkautSIS.Users.CoreModel;
using SkautSIS.Users.Models;
using SkautSIS.Users.UsersModel;

namespace SkautSIS.Users.Services
{
    public class UserCentralizationService : IUserCentralizationService
    {
        private readonly Lazy<UsersContext> _userContext;

        public UserCentralizationService(
            ISkautSisServices skautSisServices) {
            _userContext = new Lazy<UsersContext>(() => 
            {
                var serviceUrl = skautSisServices.GetServiceUrl("UsersService");
                return new UsersContext(new Uri(serviceUrl));
            });
        }

        public IEnumerable<SkautIsUserCentralizedParams> GetUserAll() 
        {
            var users = _userContext.Value.Users.AsEnumerable();
            return users.Select(u => new SkautIsUserCentralizedParams 
            {
                SkautIsPersonId = u.SkautIsPersonId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                NickName = u.NickName,
                Email = u.Email
            });
        }

        public void CreateUser(SkautIsUserCentralizedParams userParams)
        {
            if (_userContext.Value.Users.Where(u => u.SkautIsPersonId == userParams.SkautIsPersonId).FirstOrDefault() == null)
            {
                var user = new User
                {
                    SkautIsPersonId = userParams.SkautIsPersonId,
                    FirstName = userParams.FirstName,
                    LastName = userParams.LastName,
                    NickName = userParams.NickName,
                    Email = userParams.Email
                };

                _userContext.Value.AddToUsers(user);
                _userContext.Value.SaveChanges();
            }
        }

        public void UpdateUser(SkautIsUserCentralizedParams userParams) 
        {
            var user = _userContext.Value.Users.Where(u => u.SkautIsPersonId == userParams.SkautIsPersonId).FirstOrDefault();

            if (user == null) 
            {
                this.CreateUser(userParams);
                return;
            }

            user.FirstName = userParams.FirstName;
            user.LastName = userParams.LastName;
            user.NickName = userParams.NickName;
            user.Email = userParams.Email;

            _userContext.Value.UpdateObject(user);
            _userContext.Value.SaveChanges();
        }

        public void DeleteUser(int skautIsPersonId) 
        {
            var user = _userContext.Value.Users.Where(u => u.SkautIsPersonId == skautIsPersonId).FirstOrDefault();

            _userContext.Value.DeleteObject(user);
            _userContext.Value.SaveChanges();
        }

        public bool VerifyUnicity(int skautIsPersonId) 
        {
            return (_userContext.Value.Users.Where(u => u.SkautIsPersonId.Equals(skautIsPersonId)).FirstOrDefault() != null) 
                ? false : true;
        }

        public SkautIsUserCentralizedParams GetUserCentralizedData(int skautIsPersonId) 
        {
            var user = _userContext.Value.Users.Where(u => u.SkautIsPersonId == skautIsPersonId).FirstOrDefault();

            return (user == null) ? null : new SkautIsUserCentralizedParams 
                                                {
                                                    SkautIsPersonId = user.SkautIsPersonId,
                                                    FirstName = user.FirstName ?? string.Empty,
                                                    LastName = user.LastName ?? string.Empty,
                                                    NickName = user.NickName ?? string.Empty,
                                                    Email = user.Email ?? string.Empty
                                                };
        }
    }
}