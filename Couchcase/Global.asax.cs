using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Couchbase;
using Couchbase.Configuration.Client;

namespace Couchcase
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var node1Uri = ConfigurationManager.AppSettings["CouchbaseNode1"];
            var node2Uri = ConfigurationManager.AppSettings["CouchbaseNode2"];
            var node3Uri = ConfigurationManager.AppSettings["CouchbaseNode3"];

            var config = new ClientConfiguration();
            config.Servers = new List<Uri> {
                new Uri(node1Uri),
                new Uri(node2Uri),
                new Uri(node3Uri)
            };
            ClusterHelper.Initialize(config);
        }

        protected void Application_End()
        {
            ClusterHelper.Close();
        }
    }
}
