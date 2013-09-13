using Orchard.ContentManagement.Records;

namespace SkautSIS.Users.Models
{
    public class SkautSisUsersSettingsPartRecord : ContentPartRecord
    {
        /// <summary>
        /// Comma separated list of website role names that will be assigned to members of Junak.
        /// </summary>
        public virtual string RolesAssignedByMembership { get; set; }

        /// <summary>
        /// Comma separated list of "SkautIsRoleId:WebsiteRoleName:WebsiteRoleName:WebsiteRoleName..." strings 
        /// that determine website roles assigned to members of Junak based on their SkautIS roles in the 
        /// organizational unit that owns the website.
        /// </summary>
        public virtual string RolesAssignedBySkautIsRoles { get; set; }
    }
}
