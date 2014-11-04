using Orchard.Localization.Services;
using SkautSIS.Users.Models;

namespace SkautSIS.Users.ViewModels
{
    public class EditSkautIsUserViewModel
    {
        public SkautIsUserPart UserPart { get; set; }

        public IDateTimeFormatProvider DateTimeLocalization { get; set; }
    }
}