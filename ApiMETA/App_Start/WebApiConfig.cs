using ApiMETA.Models;
using System.Globalization;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ApiMETA
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));

            //Token validation
            config.MessageHandlers.Add(new ValidateTokenHandler());

            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
    }
}
