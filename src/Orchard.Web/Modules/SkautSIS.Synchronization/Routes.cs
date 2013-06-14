using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace SkautSIS.Synchronization
{
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                new RouteDescriptor() {
                    Route = new Route(
                        "admin/skautsis/synchronization-settings",
                        new RouteValueDictionary {
                            {"area", "SkautSIS.Synchronization"},
                            {"controller", "SyncSettingsAdmin"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "SkautSIS.Synchronization"}
                        },
                        new MvcRouteHandler())
                }
            };
        }
    }
}