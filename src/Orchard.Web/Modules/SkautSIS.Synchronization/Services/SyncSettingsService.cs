using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Caching;
using Orchard.ContentManagement;
using SkautSIS.Synchronization.Models;

namespace SkautSIS.Synchronization.Services
{
    public class SyncSettingsService : ISyncSettingsService
    {
        public SyncSettingsService(
            IContentManager contentManager,
            ICacheManager cacheManager)
        {
            _contentManager = contentManager;
            _cacheManager = cacheManager;
        }

        private readonly IContentManager _contentManager;
        private readonly ICacheManager _cacheManager;

        public ISyncSettings GetSycnSettingsItem() 
        {
            var settingsId = _cacheManager.Get("SyncSettingsId", ctx =>
            {
                var settings = _contentManager.Query("SyncSettings")
                    .Slice(0, 1)
                    .FirstOrDefault();

                if (settings == null) {
                    settings = _contentManager.Create<SyncSettingsPart>("SyncSettings").ContentItem;
                }

                return settings.Id;
            });

            return _contentManager.Get<ISyncSettings>(settingsId);
        }
    }
}