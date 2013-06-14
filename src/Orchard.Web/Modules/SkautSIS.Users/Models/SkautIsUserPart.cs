
using System;
using Orchard.Security;
using SkautSIS.Users.Services;

namespace SkautSIS.Users.Models
{
    using Orchard.ContentManagement;

    public class SkautIsUserPart : ContentPart<SkautIsUserPartRecord>, IUser
    {
        public const string EmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        public string UserName 
        {
            get { return Record.UserName; } 
            set { Record.UserName = value; }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }

        public string Email { get; set; }
    }
}