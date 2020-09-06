using Orchard.ContentManagement;
using System.ComponentModel.DataAnnotations;

namespace SkautSIS.Users.Models
{
    public class SkautSisUsersSettingsPart : ContentPart
    {
        [RegularExpression(@"^([\w]+\,)*[\w]+$")]
        public string RolesAssignedByMembership
        {
            get { return this.Retrieve(x => x.RolesAssignedByMembership); }
            set { this.Store(x => x.RolesAssignedByMembership, value); }
        }

        [RegularExpression(@"^([0-9]+\:([\w]+\:)*[\w]+\,)*[0-9]+\:([\w]+\:)*[\w]+$")]
        public string RolesAssignedBySkautIsRoles
        {
            get { return this.Retrieve(x => x.RolesAssignedBySkautIsRoles); }
            set { this.Store(x => x.RolesAssignedBySkautIsRoles, value); }
        }
    }
}