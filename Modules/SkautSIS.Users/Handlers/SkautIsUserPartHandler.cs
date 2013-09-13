using Orchard;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Security;
using SkautSIS.Users.Extensions;
using SkautSIS.Users.Models;
using SkautSIS.Users.Services;
using System.Web.Mvc;

namespace SkautSIS.Users.Handlers
{
    public class SkautIsUserPartHandler : ContentHandler
    {
        private const string TokenRefreshedCacheKey = "SkautSIS.Users.TokenRefreshed";
        private readonly ISkautIsUserService userService;
        private readonly IAuthenticationService authenticationService;
        private readonly IWorkContextAccessor workContextAccessor;

        public SkautIsUserPartHandler(
            IRepository<SkautIsUserPartRecord> repository,
            ISkautIsUserService userService,
            IAuthenticationService authenticationService,
            IWorkContextAccessor workContextAccessor)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<SkautIsUserPart>("User"));

            this.userService = userService;
            this.authenticationService = authenticationService;
            this.workContextAccessor = workContextAccessor;

            OnLoaded<SkautIsUserPart>(RefreshUserToken);
        }

        private void RefreshUserToken(LoadContentContext context, SkautIsUserPart part)
        {
            var workContext = this.workContextAccessor.GetContext();
            bool? tokenRefreshed = workContext.GetState<bool?>(TokenRefreshedCacheKey);

            if (!tokenRefreshed.HasValue || !tokenRefreshed.Value)
            {
                var user = this.authenticationService.GetAuthenticatedUser();

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
}