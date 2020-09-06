using System.Collections.Generic;
using System.Linq;
using SkautSIS.Users.Services;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Localization;

namespace SkautSIS.Users
{
    public class Shapes : IShapeTableProvider {

        public Shapes()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("LogOn")
                   .OnDisplaying(displaying =>
                   {
                       displaying.ShapeMetadata.Type = "SkautIsLogOn";
                   });
        }
    }
}
