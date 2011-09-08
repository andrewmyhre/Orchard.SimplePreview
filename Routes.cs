using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Mvc.Routes;
using System.Web.Routing;
using System.Web.Mvc;

namespace SimplePreview
{
    public class Routes : IRouteProvider
    {
        public void  GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor>  GetRoutes()
        {
            return new[] {
                             new RouteDescriptor {
                                 Route= new Route(
                                     "preview/{pagename}",
                                     new RouteValueDictionary{
                                         {"area", "SimplePreview"},
                                         {"controller", "Preview"},
                                         {"action","Index"}
                                     },
                                     new RouteValueDictionary(),
                                     new RouteValueDictionary() {
                                         {"area", "SimplePreview" }
                                     },
                                     new MvcRouteHandler())
                             },
                             new RouteDescriptor {
                                 Route= new Route(
                                     "published/{pagename}",
                                     new RouteValueDictionary{
                                         {"area", "SimplePreview"},
                                         {"controller", "Published"},
                                         {"action","Index"}
                                     },
                                     new RouteValueDictionary(),
                                     new RouteValueDictionary() {
                                         {"area", "SimplePreview" }
                                     },
                                     new MvcRouteHandler())
                             }
            };
        }
    }
}