using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Caching;
using Orchard.ContentManagement;
using SkautSIS.Core.Models;

namespace SkautSIS.Core.Services
{
    public class SkautSisSettingsService : ISkautSisSettingsService
    {
        public SkautSisSettingsService(
            IContentManager contentManager,
            ICacheManager cacheManager) 
        {
            _contentManager = contentManager;
            _cacheManager = cacheManager;
        }

        private readonly IContentManager _contentManager;
        private readonly ICacheManager _cacheManager;

        public ISettings GetSkautSisSettings() {

            var settingsId = _cacheManager.Get("SettingsId", ctx => 
            {
                var settings = _contentManager.Query("SkautSisSettings")
                    .Slice(0, 1)
                    .FirstOrDefault();

                if (settings == null)
                {
                    settings = _contentManager.Create<GeneralSettingsPart>("SkautSisSettings").ContentItem;
                }

                return settings.Id;
            });

            return _contentManager.Get<ISettings>(settingsId);
        }
    }
}