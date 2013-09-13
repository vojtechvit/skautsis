using Orchard.Core.Shapes.Localization;
using SkautSIS.Users.Models;

namespace SkautSIS.Users.ViewModels
{
    public class EditSkautIsUserViewModel
    {
        public SkautIsUserPart UserPart { get; set; }

        public IDateTimeLocalization DateTimeLocalization { get; set; }
    }
}