using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Mvc;
using Orchard.Environment;
using SkautSIS.Services.Core.Models;
using Autofac;

namespace SkautSIS.Services.Core.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CoreService : DataService<CoreContext>
    {

        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
            config.UseVerboseErrors = true;
        }

        [WebGet]
        public string GetServiceUrl(string serviceName) 
        {
            //var services = CurrentDataSource.Services;

            //foreach (var service in services)
            //{
            //    var s = (IServiceRegistration)service;
            //    if (s.Name.Equals(serviceName)) return s.Url;
            //}

            if (serviceName == "PersonGroupsService") return "/Services/PersonGroupsService";

            if (serviceName == "UsersService") return "/Services/UsersService";

            return string.Empty;
        }
    }

    public class CoreContext 
    {
    }
}
