using System;
using System.Web.Mvc;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.Pipeline;
using StructureMap.TypeRules;

namespace IdentityProvider.UI.Web.MVC5.DependencyResolution
{
    public class ControllerConventionOld : IRegistrationConvention
    {
        #region Public Methods and Operators

        public void ScanTypes(TypeSet types, Registry registry)
        {
            foreach (var type in types.AllTypes())
                if (type.CanBeCastTo<Controller>() && !type.IsAbstract)
                    registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
        }

        public void Process(Type type, Registry registry)
        {
            if (type.CanBeCastTo<Controller>() && !type.IsAbstract)
                registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
        }

        #endregion
    }
}