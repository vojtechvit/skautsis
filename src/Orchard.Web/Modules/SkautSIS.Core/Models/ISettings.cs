using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace SkautSIS.Core.Models
{
    public interface ISettings : IContent
    {
        int UnitId { get; }
        Guid AppId { get; }
        string SkautIsServicesUrl { get; }
        string SkautSisServicesUrl { get; }
    }
}