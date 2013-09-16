using Orchard.ContentManagement.Records;

namespace SkautSIS.Users.Models
{
    public class SkautSisCoreExtSettingsPartRecord : ContentPartRecord
    {
        public virtual int? UnitId { get; set; }

        public virtual string UnitRegistrationNumber { get; set; }

        public virtual string UnitDisplayName { get; set; }

        public virtual string UnitTypeId { get; set; }
    }
}
