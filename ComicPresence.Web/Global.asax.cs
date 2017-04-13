using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using ComicPresence.Common.Config;
using ComicPresence.Common.Logging;
using ComicPresence.Web.Controllers;
using ComicPresence.Web.Infrastructure.Ioc;

namespace ComicPresence.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string cApplicationName = "ComicPresence-CMS";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //Initialize IoC container/Unity
            BootStrapper.Initialize();

            // Registering custom controller factory
            ControllerBuilder.Current.SetControllerFactory(typeof(ControllerFactory));

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            LogManager.Instance.Configure(cApplicationName, ConfigSettings.LogFilePath);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            LoggingHelper.HandleGlobalAsaxError(Server.GetLastError());
        }
    }
}
