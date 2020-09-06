using Orchard.ContentManagement;
using System;

namespace SkautSIS.Users.Models
{
    public class SkautIsUserPart : ContentPart<SkautIsUserPartRecord>
    {
        /// <summary>
        /// The id of the user's user account in SkautIS
        /// </summary>
        public int? SkautIsUserId
        {
            get { return this.Record.SkautIsUserId; }
            set { this.Record.SkautIsUserId = value; }
        }

        /// <summary>
        /// The user's username in SkautIS
        /// </summary>
        public string SkautIsUserName
        {
            get { return this.Record.SkautIsUserName; }
            set { this.Record.SkautIsUserName = value; }
        }

        /// <summary>
        /// The id of the user's person record in SkautIS
        /// </summary>
        public int? PersonId
        {
            get { return this.Record.PersonId; }
            set { this.Record.PersonId = value; }
        }

        /// <summary>
        /// Token used for communication with SkautIS services.
        /// </summary>
        public Guid? Token
        {
            get { return this.Record.Token; }
            set { this.Record.Token = value; }
        }

        /// <summary>
        /// Date and time at which the SkautIS token is expected to expire.
        /// </summary>
        public DateTime? TokenExpiration
        {
            get { return this.Record.TokenExpiration; }
            set { this.Record.TokenExpiration = value; }
        }

        /// <summary>
        /// The id of the SkautIS role the user is currently signed to.
        /// </summary>
        /// <remarks>
        /// Value of this field is potentially unreliable! Do not use it
        /// for authorization purposes!
        /// </remarks>
        public int? RoleId
        {
            get { return this.Record.RoleId; }
            set { this.Record.RoleId = value; }
        }

        /// <summary>
        /// The id of the organizational unit that the user's currect SkautIS role
        /// is linked to.
        /// </summary>
        /// <remarks>
        /// Value of this field is potentially unreliable! Do not use it
        /// for authorization purposes!
        /// </remarks>
        public int? UnitId
        {
            get { return this.Record.UnitId; }
            set { this.Record.UnitId = value; }
        }

        /// <summary>
        /// Determines whether the user is currently a member of Junák.
        /// </summary>
        public bool? HasMembership
        {
            get { return this.Record.HasMembership; }
            set { this.Record.HasMembership = value; }
        }

        /// <summary>
        /// A comma separated list of "SkautIsUnitId:SkautIsRoleId:SkautIsRoleId:SkautIsRoleId..." strings 
        /// that determine user's SkautIS roles.
        /// </summary>
        public string SkautIsRoles
        {
            get { return this.Record.SkautIsRoles; }
            set { this.Record.SkautIsRoles = value; }
        }

        public string FirstName
        {
            get { return this.Record.FirstName; }
            set { this.Record.FirstName = value; }
        }

        public string LastName
        {
            get { return this.Record.LastName; }
            set { this.Record.LastName = value; }
        }

        public string NickName
        {
            get { return this.Record.NickName; }
            set { this.Record.NickName = value; }
        }

        public DateTime? BirthDate
        {
            get { return this.Record.BirthDate; }
            set { this.Record.BirthDate = value; }
        }
    }
}