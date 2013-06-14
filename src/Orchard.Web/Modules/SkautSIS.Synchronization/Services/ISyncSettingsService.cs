using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.ContentManagement;
using SkautSIS.Synchronization.Models;

namespace SkautSIS.Synchronization.Services
{
    public interface ISyncSettingsService : IDependency 
    {
        ISyncSettings GetSycnSettingsItem();
    }
}