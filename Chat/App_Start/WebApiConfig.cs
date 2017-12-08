using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Chat
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "api/Header/GetLogin",
                defaults: new
                {
                    controller = "Header",
                    action = "GetLogin",
                }
            );
            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "api/Header/PostLogin",
                defaults: new {
                    controller = "Header",
                    action = "PostLogin",
                }
            );
            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "api/Check/CheckMessages/{chatId}/{lastMsgId}",
                defaults: new
                {
                    controller = "Check",
                    action = "CheckMessages",
                    chatId = RouteParameter.Optional,
                    lastMsgId = RouteParameter.Optional
                }
            );
            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "api/Check/CheckBids",
                defaults: new
                {
                    controller = "Check",
                    action = "CheckBids"
                }
            );
            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "api/Check/CheckChats",
                defaults: new
                {
                    controller = "Check",
                    action = "CheckChats"
                }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}",
                defaults: new { 
                }
            );
        }
    }
}
