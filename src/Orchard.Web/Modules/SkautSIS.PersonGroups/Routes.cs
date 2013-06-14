using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace SkautSIS.PersonGroups {
    public class Routes : IRouteProvider {

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "Admin/PersonGroups",
                        new RouteValueDictionary {
                            {"area", "SkautSIS.PersonGroups"},
                            {"controller", "PersonGroupsAdmin"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "SkautSIS.PersonGroups"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "Admin/PersonGroups/{id}",
                        new RouteValueDictionary {
                            {"area", "SkautSIS.PersonGroups"},
                            {"controller", "PersonGroupsAdmin"},
                            {"action", "Details"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "SkautSIS.PersonGroups"}
                        },
                        new MvcRouteHandler())
                }
                //,
                //new RouteDescriptor {
                //    Priority = 5,
                //    Route = new Route(
                //        "Admin/PersonGroups/New",
                //        new RouteValueDictionary {
                //            {"area", "SkautSIS.PersonGroups"},
                //            {"controller", "PersonGroupsAdmin"},
                //            {"action", "Create"}
                //        },
                //        new RouteValueDictionary(),
                //        new RouteValueDictionary {
                //            {"area", "SkautSIS.PersonGroups"}
                //        },
                //        new MvcRouteHandler())
                //}
            };
        }
    }
}