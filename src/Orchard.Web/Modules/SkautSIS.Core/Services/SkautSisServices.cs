using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using SkautSIS.Core.CoreModel;
using SkautSIS.Core.Models;

namespace SkautSIS.Core.Services
{
    public class SkautSisServices : ISkautSisServices
    {
        private readonly IContentManager _contentManager;
        private readonly IOrchardServices _orchardServices;
        private readonly ISkautSisSettingsService _settingsService;
        private readonly ISkautIsAuthorizer _authorizer;
        public Localizer T { get; set; }

        public SkautSisServices(
            IContentManager contentManager,
            ISkautSisSettingsService settingsService,
            ISkautIsAuthorizer authorizer,
            IOrchardServices orchardServices)
        {
            _authorizer = authorizer;
            _settingsService = settingsService;
            _contentManager = contentManager;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public ISkautIsAuthorizer SkautIsAuthorizer {
            get { return _authorizer; }
        }

        public ISettings SkautSisSettings 
        { 
            get { return _settingsService.GetSkautSisSettings(); } 
        }

        public string GetServiceUrl(string serviceName) 
        {
            var serviceUrl = this.SkautSisSettings.SkautSisServicesUrl;
            return serviceUrl + "/SkautSisServices/" + serviceName;
        }
    }
}