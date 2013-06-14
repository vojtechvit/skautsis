using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement.Records;

namespace SkautSIS.Users.Models
{
    public class RegistrationSettingsPartRecord : ContentPartRecord 
    {
        public virtual bool UsersCanRegister { get; set; }
        public virtual bool EnableLostPassword { get; set; }
    }
}