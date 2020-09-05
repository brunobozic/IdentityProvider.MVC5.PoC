using StructureMap;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace IdentityProvider.Controllers
{
    public class IoCControllerFactory : DefaultControllerFactory
    {
        private const string StructuremapNestedContainerKey = "Nested.Container.Key";

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                if (controllerType != null)
                {
                    if (requestContext != null)
                        if (requestContext.HttpContext.Items.Contains(StructuremapNestedContainerKey))
                        {
                            var myContainer =
                                (IContainer)requestContext.HttpContext.Items[StructuremapNestedContainerKey];
                            return myContainer.GetInstance(controllerType) as IController;
                        }
                    return DependencyResolver.Current.GetService(controllerType) as IController;
                }
                return null;
            }
            catch (Exception ex)
            {
                // DependencyResolver.Current.GetService<ILog4NetLoggingService>().LogFatal(this, "Fatal", ex);
                return null;
            }
        }
    }
}