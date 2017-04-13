using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using ComicPresence.Common.Logging;
using ComicPresence.Web.Infrastructure.Containers;

namespace ComicPresence.Web.Controllers
{
    public class ControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                if (controllerType == null)
                {
                    LogManager.Instance.Log(string.Format(ComicResource.CmsControllerNull, controllerType.FullName), LoggingLevel.Warn);
                    throw new ArgumentNullException("controllerType");
                }

                if (!typeof(IController).IsAssignableFrom(controllerType))
                {
                    LogManager.Instance.Log(string.Format(ComicResource.CmsControllerTypeMissMatchError, controllerType.Name), LoggingLevel.Error);

                    throw new ArgumentException(string.Format(ComicResource.CmsControllerTypeMissMatchError, controllerType.Name), "controllerType");
                }

                return CPUnityContainer.Container.Resolve(controllerType, controllerType.Name) as IController;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(string.Format(ComicResource.CmsError, ex.Message.ToString()), ex);
                return null;
            }
        }
    }
}