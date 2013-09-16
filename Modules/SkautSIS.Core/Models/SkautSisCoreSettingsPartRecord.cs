using Orchard.ContentManagement.Records;
using System;

namespace SkautSIS.Core.Models
{
    public class SkautSisCoreSettingsPartRecord : ContentPartRecord
    {
        public virtual Guid? AppId { get; set; }

        public virtual bool UseTestingWebServices { get; set; }
    }
}
