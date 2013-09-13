using Orchard.ContentManagement.Records;
using System;

namespace SkautSIS.Users.Models
{
    public class SkautIsUserPartRecord : ContentPartRecord
    {
        public virtual int? SkautIsUserId { get; set; }

        public virtual string SkautIsUserName { get; set; }

        public virtual int? PersonId { get; set; }
        
        public virtual Guid? Token { get; set; }

        public virtual DateTime? TokenExpiration { get; set; }

        public virtual int? RoleId { get; set; }

        public virtual int? UnitId { get; set; }

        public virtual bool? HasMembership { get; set; }

        /// <summary>
        /// A comma separated list of "SkautIsUnitId:SkautIsRoleId:SkautIsRoleId:SkautIsRoleId..." strings 
        /// that determine user's SkautIS roles.
        /// </summary>
        public virtual string SkautIsRoles { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string NickName { get; set; }

        public virtual DateTime? BirthDate { get; set; }
    }
}