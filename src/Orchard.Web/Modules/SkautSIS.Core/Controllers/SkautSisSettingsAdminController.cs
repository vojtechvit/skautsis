using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Caching;
using Orchard.Localization;
using Orchard.Security.Permissions;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using SkautSIS.Core.Models;
using Orchard.ContentManagement;
using SkautSIS.Core.Services;

namespace SkautSIS.Core.Controllers
{
    [Admin]
    public class SkautSisSettingsAdminController : Controller, IUpdateModel
    {
        public SkautSisSettingsAdminController(
            IOrchardServices services,
            ISkautSisSettingsService settingsService) 
        {
            T = NullLocalizer.Instance;
            _services = services;
            _settingsService = settingsService;
        }

        public Localizer T { get; set; }
        private readonly IOrchardServices _services;
        private readonly ISkautSisSettingsService _settingsService;

        public ActionResult Index() 
        {
            if (!_services.Authorizer.Authorize(Persmissions.ManageSkautSisSettings, T("Not authorized to manage SkautSIS configuration.")))
                return new HttpUnauthorizedResult();

            dynamic model = _services.ContentManager.BuildEditor(_settingsService.GetSkautSisSettings());

            return View((object)model);
        }

        [HttpPost, ActionName("Index")]
        public ActionResult IndexPOST()
        {
            if (!_services.Authorizer.Authorize(Persmissions.ManageSkautSisSettings, T("Not authorized to manage SkautSIS configuration.")))
                return new HttpUnauthorizedResult();

            var settings = _settingsService.GetSkautSisSettings();
            dynamic model = _services.ContentManager.UpdateEditor(settings, this);

            if (!ModelState.IsValid) {
                _services.TransactionManager.Cancel();
                return View((object) model);
            }

            _services.Notifier.Information(T("Settings updated"));
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