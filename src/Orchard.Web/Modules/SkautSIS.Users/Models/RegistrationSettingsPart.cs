using Orchard.ContentManagement;

namespace SkautSIS.Users.Models 
{
    public class RegistrationSettingsPart : ContentPart<RegistrationSettingsPartRecord> 
    {
        public bool UsersCanRegister {
            get { return Record.UsersCanRegister; }
            set { Record.UsersCanRegister = value; }
        }

        public bool EnableLostPassword {
            get { return Record.EnableLostPassword; }
            set { Record.EnableLostPassword = value; }
        }

    }
}