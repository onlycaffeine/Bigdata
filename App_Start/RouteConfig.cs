using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace testmongo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}",
            new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
            name: "Product Category",
            url: "san-pham/{metatitle}-{cateId}",
            defaults: new { controller = "ShowProduct", action = "Category", id = UrlParameter.Optional },
            namespaces: new[] { "testmongo.Controllers" }
            );

            routes.MapRoute(
              name: "Product Detail",
              url: "chi-tiet/{metatitle}-{id}",
              defaults: new { controller = "ShowProduct", action = "Detail", id = UrlParameter.Optional },
              namespaces: new[] { "testmongo.Controllers" }
            );

            routes.MapRoute(
               name: "Saled",
               url: "khuyen-mai",
               defaults: new { controller = "Home", action = "List", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
           );

            routes.MapRoute(
               name: "Feature",
               url: "ban-chay",
               defaults: new { controller = "ShowProduct", action = "Chart", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
           );

            routes.MapRoute(
               name: "Acount",
               url: "tai-khoan",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
           );

            routes.MapRoute(
               name: "Search",
               url: "tim-kiem",
               defaults: new { controller = "ShowProduct", action = "Search", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
           );

            routes.MapRoute(
               name: "Login",
               url: "{Home}/dang-nhap",
               defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
           );

            routes.MapRoute(
               name: "ReLogin",
               url: "dang-nhap",
               defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
           );

            routes.MapRoute(
               name: "About",
               url: "gioi-thieu",
               defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
           );

            routes.MapRoute(
               name: "FAQS",
               url: "faqs",
               defaults: new { controller = "FAQS", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
           );

            routes.MapRoute(
               name: "Contact",
               url: "lien-he",
               defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
           );

            routes.MapRoute(
               name: "Rate",
               url: "danh-gia/{id}",
               defaults: new { controller = "User", action = "Rate", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
            );

            routes.MapRoute(
              name: "History",
              url: "lich-su",
              defaults: new { controller = "User", action = "History", id = UrlParameter.Optional },
              namespaces: new[] { "testmongo.Controllers" }
            );

            routes.MapRoute(
              name: "Cart",
              url: "gio-hang",
              defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "testmongo.Controllers" }
            );

            routes.MapRoute(
                name: "Payment",
                url: "thanh-toan",
                defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
);

            routes.MapRoute(
               name: "Add Cart",
               url: "them-gio-hang",
               defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
           );

            routes.MapRoute(
            name: "Payment Success",
                url: "hoan-thanh",
                defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
);

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Shop", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "testmongo.Controllers" }
           );
        }
    }
}