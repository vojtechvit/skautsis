using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkautSIS.Core.Models
{
    public class UnitRecord {
        public virtual string SkautIsUnitId { get; set; }

        public virtual string DisplayName { get; set; }
        public virtual string RegistrationNumber { get; set; }
        public virtual string ShortRegistrationNumber { get; set; }
        public virtual string Location { get; set; }
        public virtual string IC { get; set; }
        public virtual string DIC { get; set; }

        public virtual string Street { get; set; }
        public virtual string City { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string State { get; set; }

        public virtual string PostalFirstLine { get; set; }
        public virtual string PostalStreet { get; set; }
        public virtual string PostalCity { get; set; }
        public virtual string PostalPostcode { get; set; }
        public virtual string PostalState { get; set; }

        public virtual string Note { get; set; }

        public virtual string UnitType { get; set; }
        public virtual string UnitTypeNote { get; set; }
    }
}