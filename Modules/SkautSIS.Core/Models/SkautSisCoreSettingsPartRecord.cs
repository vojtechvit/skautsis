using Orchard.ContentManagement.Records;
using System;

namespace SkautSIS.Core.Models
{
    public class SkautSisCoreSettingsPartRecord : ContentPartRecord
    {
        public virtual Guid? AppId { get; set; }

        public virtual bool UseTestingWebServices { get; set; }

        public virtual int? UnitId { get; set; }

        public virtual string UnitRegistrationNumber { get; set; }

        public virtual string UnitDisplayName { get; set; }
    }
}
