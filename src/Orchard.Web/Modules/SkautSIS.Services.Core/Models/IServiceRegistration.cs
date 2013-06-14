using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;

namespace SkautSIS.Services.Core.Models
{
    public interface IServiceRegistration : IDependency
    {
        string Name { get; }
        string VirtualPath { get; }
    }
}