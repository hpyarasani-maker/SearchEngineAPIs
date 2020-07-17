using SearchEngineAPIs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SearchEngineAPIs
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.Filters.Add(new BasicAuthenticationAttribute());

            // Web API routes
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",

                routeTemplate: "trackingApp/{controller}/{id}",

                defaults: new { id = RouteParameter.Optional }

            );
            GlobalConfiguration.Configuration.MessageHandlers.Add(new BasicAuthenticationHandler());

        }
    }
}
