using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using SkautSIS.Core.Models;

namespace SkautSIS.Core.Services
{
    public interface ISkautSisSettingsService : IDependency 
    {
        ISettings GetSkautSisSettings();
    }
}