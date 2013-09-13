using Orchard.ContentManagement;
using System.ComponentModel.DataAnnotations;

namespace SkautSIS.Users.Models
{
    public class SkautSisUsersSettingsPart : ContentPart<SkautSisUsersSettingsPartRecord>
    {
        [RegularExpression(@"^([\w]+\,)*[\w]+$")]
        public string RolesAssignedByMembership
        {
            get { return this.Record.RolesAssignedByMembership; }
            set { this.Record.RolesAssignedByMembership = value; }
        }

        [RegularExpression(@"^([0-9]+\:([\w]+\:)*[\w]+\,)*[0-9]+\:([\w]+\:)*[\w]+$")]
        public string RolesAssignedBySkautIsRoles
        {
            get { return this.Record.RolesAssignedBySkautIsRoles; }
            set { this.Record.RolesAssignedBySkautIsRoles = value; }
        }
    }
}