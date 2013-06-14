using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using SkautSIS.Synchronization.Services;

namespace SkautSIS.Synchronization.Controllers
{
    [Admin]
    public class SyncSettingsAdminController : Controller, IUpdateModel
    {
        public SyncSettingsAdminController(
            IOrchardServices services,
            ISyncSettingsService syncSettingsService,
            IComponentContext componentContext) 
        {
            T = NullLocalizer.Instance;
            _services = services;
            _syncSettingsService = syncSettingsService;
            _componentContext = componentContext;
        }
        
        public Localizer T { get; set; }
        private readonly IOrchardServices _services;
        private readonly ISyncSettingsService _syncSettingsService;
        private readonly IComponentContext _componentContext;
        private IEnumerable<ISynchronizationService> _syncServices;

        public ActionResult Index() 
        {
            if (!_services.Authorizer.Authorize(Persmissions.ManageSynchronizationSettings, T("Not authorized to manage synchronization configuration.")))
                return new HttpUnauthorizedResult();

            dynamic model = _services.ContentManager.BuildEditor(_syncSettingsService.GetSycnSettingsItem());

            return View((object)model);
        }
        
        [HttpPost, ActionName("Index")]
        public ActionResult IndexPOST()
        {
            if (!_services.Authorizer.Authorize(Persmissions.ManageSynchronizationSettings, T("Not authorized to manage synchronization configuration.")))
                return new HttpUnauthorizedResult();

            var settings = _syncSettingsService.GetSycnSettingsItem();
            dynamic model = _services.ContentManager.UpdateEditor(settings, this);

            if (!ModelState.IsValid) {
                _services.TransactionManager.Cancel();
                return View((object) model);
            }

            _services.Notifier.Information(T("Synchronization settings updated"));

            return RedirectToAction("Index");
        }

        public ActionResult Synchronize()
        {
            if (!_services.Authorizer.Authorize(Persmissions.PerformSynchronization, T("Not authorized to manage synchronization configuration.")))
                return new HttpUnauthorizedResult();
            try {
                //TODO: zjistit skautIS user role a pripadne se ji pokusit zmenit 
                //if (!_skautSisServices.SkautIsAuthorizer.Authorize())
                //    return new HttpUnauthorizedResult();


                if (_syncServices == null) {
                    _syncServices = _componentContext.Resolve<IEnumerable<ISynchronizationService>>();
                }

                foreach (var service in _syncServices) {
                    if (!service.Synchronize()) {
                        _services.Notifier.Error(T("Synchronization was unsuccessfull"));
                    }
                }
            } catch(Exception e) {
                _services.Notifier.Error(T("An error occured during synchronization with SkautIS: {0}", e.Message));
            }

            return RedirectToAction("Index");
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) 
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}