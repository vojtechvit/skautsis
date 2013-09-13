using Orchard.ContentManagement;

namespace SkautSIS.Users.Models
{
    public class SkautSisCoreExtSettingsPart : ContentPart<SkautSisCoreExtSettingsPartRecord>
    {
        public string UnitTypeId
        {
            get { return this.Record.UnitTypeId; }
            set { this.Record.UnitTypeId = value; }
        }
    }
}