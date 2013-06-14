using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using SkautSIS.Services.Users.Models;


namespace SkautSIS.Services.Users.Services
{
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]   
    public class UsersService : DataService<UsersContext>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.AllRead);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
           // config.UseVerboseErrors = true;
        }

        [WebGet]
        public IQueryable<User> GetUsers(string skautIsUsersIds)
        {
            var query = CurrentDataSource.Users as IQueryable<User>;

            // TODO logic for getting ids from string UserIds

            return query.Where(u => u.SkautIsPersonId == 10); 
        }
    }
}
