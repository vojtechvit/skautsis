using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SkautSIS.Services.Users.Models
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UsersRelationship> UsersRelationship { get; set; }
        public DbSet<UsersRelationshipRequest> UsersRelationshipRequest { get; set; }
    }
}