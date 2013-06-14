using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace SkautSIS.Users
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
                        "Admin/uzivatele",
                        new RouteValueDictionary {
                            {"area", "SkautSIS.Users"},
                            {"controller", "AccountAdmin"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "SkautSIS.Users"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor() {
                    Route = new Route(
                        "uzivatel/prihlasit",
                        new RouteValueDictionary {
                            {"area", "SkautSIS.Users"},
                            {"controller", "Account"},
                            {"action", "LogOn"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "SkautSIS.Users"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor() {
                    Route = new Route(
                        "uzivatel/odhlasit",
                        new RouteValueDictionary {
                            {"area", "SkautSIS.Users"},
                            {"controller", "Account"},
                            {"action", "LogOff"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "SkautSIS.Users"}
                        },
                        new MvcRouteHandler())
                }
            };
        }
    }
}