﻿using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using Orchard.Wcf;
using SkautSIS.Services.Users.Services;

namespace SkautSIS.Services.Users
{
    public class Routes : IRouteProvider
    {
        private static ServiceRoute _route = new ServiceRoute("SkautSisServices/UsersService",
                                                       new DataServiceHostFactory(),
                                                       typeof(UsersService));

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                    new RouteDescriptor {   Priority = -1000,
                                            Route = _route
                    }
                };
        }
    }
}