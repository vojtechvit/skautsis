using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Data.Common;

namespace SkautSIS.Services.PersonGroups.Models
{
    public class PersonGroupsContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonGroup> PersonGroups { get; set; }

        public PersonGroupsContext()
        {
            Database.SetInitializer(new PersonGroupsInitializer());
        }

        //private static string ConnectionString {
        //    get {
        //        var builder = new DbConnectionStringBuilder();
        //        builder.Add("Provider", "Provider=System.Data.SqlServerCe.4.0");
        //        builder.Add("Data Source", "|DataDirectory|PersonGroupsDatabase.sdf");

        //        return builder.ConnectionString;
        //    }
        //}
    }

    

    public class PersonGroupsInitializer : DropCreateDatabaseIfModelChanges<PersonGroupsContext>
    {
        protected override void Seed(PersonGroupsContext context)
        {
            
            #region PersonGroups

            var personGroups = new List<PersonGroup> {
                new PersonGroup() {
                    PersonGroupId = 0,
                    DisplayName = "Group 1",
                    Type = 0
                },
                new PersonGroup() {
                    PersonGroupId = 1,
                    DisplayName = "Group 2",
                    Type = 1
                },
                new PersonGroup() {
                    PersonGroupId = 2,
                    DisplayName = "Group 3",
                    Type = 2
                }
            };

            personGroups.ForEach(grp => context.PersonGroups.Add(grp));

            #endregion

        }
    }
    
}