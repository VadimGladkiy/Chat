using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Chat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "",
                url: "Home/IsAuthorizedUser",
                defaults: new
                {
                    controller = "Home",
                    action = "IsAuthorizedUser",
                }
            );
            routes.MapRoute(
                name: "",
                url: "Home/Conversation/{chat_id}",
                defaults: new
                {
                    controller = "Home",
                    action = "Conversation",
                    chat_id = UrlParameter.Optional
                }
            );
            routes.MapRoute(
                name: "",
                url: "Home/Index",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                }
            );
            routes.MapRoute(
                name: "",
                url: "Home/Register",
                defaults: new
                {
                    controller = "Home",
                    action = "Register"
                }
            );
            routes.MapRoute(
                name: "",
                url: "Friend/Friends",
                defaults: new
                {
                    controller = "Friend",
                    action = "FriendList"
                }
            );
            routes.MapRoute(
                name: "",
                url: "Friend/SearchFriend/{persName}",
                defaults: new
                {
                    controller = "Friend",
                    action = "SearchFriend",
                    persName = UrlParameter.Optional
                }
            );
            routes.MapRoute(
               name: "",
               url: "Friend/SetContact/{cont_id}",
               defaults: new
               {
                   controller = "Friend",
                   action = "SetContact",
                   cont_id = UrlParameter.Optional
               }
           );
            routes.MapRoute(
               name: "",
               url: "Friend/Bids",
               defaults: new
               {
                   controller = "Friend",
                   action = "Bids",
               }
           );
            routes.MapRoute(
               name: "",
               url: "Friend/AcceptBid/{applicant_id}",
               defaults: new
               {
                   controller = "Friend",
                   action = "AcceptBid",
                   applicant_id = UrlParameter.Optional
               }
           );
            routes.MapRoute(
               name: "",
               url: "Friend/DenyBid/{applicant_id}",
               defaults: new
               {
                   controller = "Friend",
                   action = "DenyBid",
                   applicant_id = UrlParameter.Optional
               }
           );
            routes.MapRoute(
               name: "",
               url: "Friend/RemoveFriend/{comrad_id}",
               defaults: new
               {
                   controller = "Friend",
                   action = "RemoveFriend",
                   comrad_id = UrlParameter.Optional
               }
           );
            routes.MapRoute(
               name: "",
               url: "Notes/CreateChat{friend_id}",
               defaults: new
               {
                   controller = "Notes",
                   action = "CreateChat",
                   friend_id = UrlParameter.Optional
               }
           );
            routes.MapRoute(
               name: "",
               url: "Notes/LoadChat/{chat_id}",
               defaults: new
               {
                   controller = "Notes",
                   action = "LoadChat",
                   chat_id = UrlParameter.Optional
               }
           );
            routes.MapRoute(
               name: "",
               url: "Notes/WriteMessage",
               defaults: new
               {
                   controller = "Notes",
                   action = "WriteMessage",
                   //chat_id = UrlParameter.Optional
               }
           );
            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                }
            );
        }
    }
}
