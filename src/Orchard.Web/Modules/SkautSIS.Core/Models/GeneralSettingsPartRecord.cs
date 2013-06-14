using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Records;

namespace SkautSIS.Core.Models
{
    public class GeneralSettingsPartRecord : ContentPartRecord
    {
        public virtual int UnitId { get; set; }
        public virtual Guid AppId { get; set; }
        public virtual string SkautIsServicesUrl { get; set; }
        public virtual string SkautSisServicesUrl { get; set; }
    }
}
