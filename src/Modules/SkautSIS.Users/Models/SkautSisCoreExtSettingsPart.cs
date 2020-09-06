using Orchard.ContentManagement;
using System.ComponentModel.DataAnnotations;

namespace SkautSIS.Users.Models
{
    public class SkautSisCoreExtSettingsPart : ContentPart
    {
        public int? UnitId
        {
            get { return string.IsNullOrEmpty(this.Retrieve<string>("UnitId")) ? (int?)null : int.Parse(this.Retrieve<string>("UnitId")); }
            set { this.Store("UnitId", value.HasValue ? value.Value.ToString() : null); }
        }

        [RegularExpression(@"^([0-9]+\.)*[0-9]+$")]
        public string UnitRegistrationNumber
        {
            get { return this.Retrieve(x => x.UnitRegistrationNumber); }
            set { this.Store(x => x.UnitRegistrationNumber, value); }
        }

        public string UnitDisplayName
        {
            get { return this.Retrieve(x => x.UnitDisplayName); }
            set { this.Store(x => x.UnitDisplayName, value); }
        }

        public string UnitTypeId
        {
            get { return this.Retrieve(x => x.UnitTypeId); }
            set { this.Store(x => x.UnitTypeId, value); }
        }
    }
}