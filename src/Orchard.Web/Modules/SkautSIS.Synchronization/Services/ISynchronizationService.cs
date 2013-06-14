using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using SkautSIS.Synchronization.Models;

namespace SkautSIS.Synchronization.Services
{
    public interface ISynchronizationService :IDependency {
        bool Synchronize();
    }
}