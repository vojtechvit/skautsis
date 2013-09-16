using Orchard.ContentManagement;
using System.ComponentModel.DataAnnotations;

namespace SkautSIS.Users.Models
{
    public class SkautSisCoreExtSettingsPart : ContentPart<SkautSisCoreExtSettingsPartRecord>
    {
        public int? UnitId
        {
            get { return this.Record.UnitId; }
            set { this.Record.UnitId = value; }
        }

        [RegularExpression(@"^([0-9]+\.)*[0-9]+$")]
        public string UnitRegistrationNumber
        {
            get { return this.Record.UnitRegistrationNumber; }
            set { this.Record.UnitRegistrationNumber = value; }
        }

        public string UnitDisplayName
        {
            get { return this.Record.UnitDisplayName; }
            set { this.Record.UnitDisplayName = value; }
        }

        public string UnitTypeId
        {
            get { return this.Record.UnitTypeId; }
            set { this.Record.UnitTypeId = value; }
        }
    }
}