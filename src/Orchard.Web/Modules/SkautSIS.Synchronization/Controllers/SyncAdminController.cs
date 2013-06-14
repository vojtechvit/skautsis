using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Orchard;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using SkautSIS.Synchronization.Services;

namespace SkautSIS.Synchronization.Controllers
{
    [Admin]
    public class SyncAdminController : Controller
    {
        private readonly IComponentContext _componentContext;
        private readonly IOrchardServices _orchardServices;
        private IEnumerable<ISynchronizationService> _syncServices;
        public Localizer T;

        public SyncAdminController(
            IOrchardServices orchardServices,
            IComponentContext componentContext) 
        {
            _orchardServices = orchardServices;
            _componentContext = componentContext;
            T = NullLocalizer.Instance;
        }

        public ActionResult Synchronize() 
        {
            //if (!_orchardServices.Authorizer.Authorize(Persmissions.SynchronizeData, T("Not authorized to manage synchronization configuration.")))
            //    return new HttpUnauthorizedResult();

            //TODO: zjistit skautIS user role a pripadne se ji pokusit zmenit 
            //if (!_skautSisServices.SkautIsAuthorizer.Authorize())
            //    return new HttpUnauthorizedResult();


            //if (_syncServices == null) {
            //    _syncServices = _componentContext.Resolve<IEnumerable<ISynchronizationService>>();
            //}

            //perfrom synchronization

            return View();
        }
    }
}