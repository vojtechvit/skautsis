using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Localization;

namespace SkautSIS.Core.Services
{
    public interface ISkautIsAuthorizer : IDependency
    {
        bool Authorize(IList<string> skautIsRoles);
        bool Authorize(IList<string> skautIsRoles, LocalizedString message);
    }
}