using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security;
using SkautSIS.Users.Models;

namespace SkautSIS.Users.ViewModels
{
    public class UsersIndexViewModel
    {
        public IList<UserEntry> Users { get; set; }
        public UserIndexOptions Options { get; set; }
    }


    public class UserEntry
    {
        public SkautIsUserCentralizedParams User { get; set; }
        public bool IsChecked { get; set; }
    }

    public class UserIndexOptions
    {
        public string Search { get; set; }
        public UsersOrder Order { get; set; }
        public UsersFilter Filter { get; set; }
    }

    public enum UsersOrder
    {
        Name,
        NickName,
        UserName,
        Email
    }

    public enum UsersFilter
    {
        All,
        Approved,
        Pending,
        EmailPending
    }
}