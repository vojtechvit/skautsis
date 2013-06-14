using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace SkautSIS.Users.Models
{
    public class SkautIsUserDetailPart : ContentPart
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsForeign { get; set; }
        public int YearFrom { get; set; }
    }
}