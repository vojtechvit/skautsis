using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using SkautSIS.Core.Models;

namespace SkautSIS.Core.Services
{
    public interface ISkautSisServices : IDependency
    {
        ISkautIsAuthorizer SkautIsAuthorizer { get; }
        ISettings SkautSisSettings { get; }
        string GetServiceUrl(string serviceName);
    }
}