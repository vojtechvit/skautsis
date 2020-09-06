using Orchard.Security;
using Orchard.Users.Events;
using Orchard.ContentManagement;
using SkautSIS.Users.Services;
using SkautSIS.Users.Models;

namespace SkautSIS.Users.Events
{
    public class SkautIsUserEventHandler : IUserEventHandler
    {
        private readonly ISkautIsUserService userService;

        public SkautIsUserEventHandler(
            ISkautIsUserService userService)
        {
            this.userService = userService;
        }

        public void Creating(UserContext context)
        { }

        public void Created(UserContext context)
        {
            this.userService.UpdateUserInfo(context.User);
        }

        public void LoggedIn(IUser user)
        {
            this.userService.UpdateUserInfo(user);
        }
        
        public void LoggedOut(IUser user)
        {
            this.userService.InvalidateLoginData(user);
        }

        public void AccessDenied(IUser user)
        { }

        public void ChangedPassword(IUser user)
        { }

        public void SentChallengeEmail(IUser user)
        { }

        public void ConfirmedEmail(IUser user)
        { }

        public void Approved(IUser user)
        { }
    }
}