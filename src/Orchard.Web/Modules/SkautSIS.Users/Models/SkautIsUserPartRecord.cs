using System;
using Orchard.ContentManagement.Records;

namespace SkautSIS.Users.Models
{
    public class SkautIsUserPartRecord : ContentPartRecord
    {
        public virtual int SkautIsUserId { get; set; }
        public virtual int SkautIsPersonId { get; set; }
        public virtual string UserName { get; set; }
        public virtual int? SkautIsRoleId { get; set; }
        public virtual int? SkautIsUnitId { get; set; }
        public virtual Guid? SkautIsToken { get; set; }
        public virtual DateTime? SkautIsTokenExpiration { get; set; }
    }
}