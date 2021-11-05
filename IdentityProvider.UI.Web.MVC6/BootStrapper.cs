using IdentityProvider.Infrastructure.Email;
using StructureMap;


namespace IdentityProvider.UI.Web.MVC5
{
    public class BootStrapper
    {
        public class ControllerRegistry : Registry
        {
            public ControllerRegistry()
            {

                // Handlers for Domain Events
                // For<IDomainEventHandlerFactory>().Use<StructureMapDomainEventHandlerFactory>();
                //For<IDomainEventHandler<OrderSubmittedEvent>>()
                //	   .AddConcreteType<OrderSubmittedHandler>();


                // Product Catalogue & Category Service with Caching Layer Registration
                //this.InstanceOf<IProductCatalogService>().Is.OfConcreteType<ProductCatalogService>()
                //	.WithName("RealProductCatalogueService");

                // Uncomment the line below to use the product service caching layer
                //For<IProductCatalogueService>().Use<CachedProductCatalogueService>()
                //    .CtorDependency<IProductCatalogueService>().Is(x => x.TheInstanceNamed("RealProductCatalogueService"));

       

                // Email Service                 
                For<IEmailService>().Use<TextLoggingEmailService>();

            }
        }
    }
}