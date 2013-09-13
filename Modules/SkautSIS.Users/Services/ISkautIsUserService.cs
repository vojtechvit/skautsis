using Orchard;
using Orchard.Security;
using System;

namespace SkautSIS.Users.Services
{
    public interface ISkautIsUserService : IDependency
    {
        bool IsSkautIsUser();
        bool IsSkautIsUser(IUser user);

        void RefreshToken();
        void RefreshToken(IUser user);

        bool HasValidToken();
        bool HasValidToken(IUser user);

        void InvalidateLoginData();
        void InvalidateLoginData(IUser user);

        void UpdateUserInfo();
        void UpdateUserInfo(IUser user);
        void UpdateUserInfo(IUser user, Guid token);

        IUser GetOrCreateUser(Guid userToken);
        IUser GetOrCreateUser(int skautIsUserId);
        IUser GetOrCreateUser(int skautIsUserId, Guid token);
    }
}