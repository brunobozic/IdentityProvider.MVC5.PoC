using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Email;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using StructureMap;
using System.Data.Entity;


namespace IdentityProvider.UI.Web.MVC5
{
    public class BootStrapper
    {
        public class ControllerRegistry : Registry
        {
            public ControllerRegistry()
            {
                // Repositories 
                //For<IOrderRepository>().Use
                //		 <GenericRepository.NHibernate.Repositories.OrderRepository>();
                //For<ICustomerRepository>().Use
                //		 <GenericRepository.NHibernate.Repositories.CustomerRepository>();
                //For<IBasketRepository>().Use
                //		 <GenericRepository.NHibernate.Repositories.BasketRepository>();
                //For<IDeliveryOptionRepository>().Use
                //		  <GenericRepository.NHibernate.Repositories.DeliveryOptionRepository>();

                //For<ICategoryRepository>().Use
                //		 <GenericRepository.NHibernate.Repositories.CategoryRepository>();
                //For<IProductTitleRepository>().Use
                //		 <GenericRepository.NHibernate.Repositories.ProductTitleRepository>();
                //For<IProductRepository>().Use
                //		 <GenericRepository.NHibernate.Repositories.ProductRepository>();
                //For<IUnitOfWork>().Use
                //		 <GenericRepository.NHibernate.NHUnitOfWork>();

                // Order Service
                //For<IOrderService>().Use
                //		<OrderService>();

                // Payment
                //For<IPaymentService>().Use
                //		<PayPalPaymentService>();

                // Handlers for Domain Events
                // For<IDomainEventHandlerFactory>().Use<StructureMapDomainEventHandlerFactory>();
                //For<IDomainEventHandler<OrderSubmittedEvent>>()
                //	   .AddConcreteType<OrderSubmittedHandler>();


                // Product Catalogue                                         
                //For<IProductCatalogService>().Use
                //		 <ProductCatalogService>();

                // Product Catalogue & Category Service with Caching Layer Registration
                //this.InstanceOf<IProductCatalogService>().Is.OfConcreteType<ProductCatalogService>()
                //	.WithName("RealProductCatalogueService");

                // Uncomment the line below to use the product service caching layer
                //For<IProductCatalogueService>().Use<CachedProductCatalogueService>()
                //    .CtorDependency<IProductCatalogueService>().Is(x => x.TheInstanceNamed("RealProductCatalogueService"));

       

                // Email Service                 
                For<IEmailService>().Use<TextLoggingEmailService>();


                For<IUserStore<IdentityUser>>()
                    .Use<UserStore<IdentityUser>>()
                    .Ctor<DbContext>()
                    .Is<IdentityDbContext>(cfg => cfg
                        .SelectConstructor(() => new IdentityDbContext("DefaultConnection")).Ctor<string>()
                        .Is("IdentitySetupWithStructureMap"));

                ForConcreteType<UserManager<IdentityUser>>()
                    .Configure
                    .SetProperty(userManager => userManager.PasswordValidator = new PasswordValidator
                    {
                        RequiredLength = 6
                    })
                    .SetProperty(
                        userManager => userManager.UserValidator = new UserValidator<IdentityUser>(userManager));
                //For<ICustomerService>().Use
                //		<CustomerService>();

                // Authentication
                //For<IExternalAuthenticationService>().Use
                //	<JanrainAuthenticationService>();
                //For<IFormsAuthentication>().Use
                //	<AspFormsAuthentication>();
                //For<ILocalAuthenticationService>().Use
                //	<AspMembershipAuthentication>();

                // Controller Helpers
                //For<IActionArguments>().Use
                //	 <HttpRequestActionArguments>();
            }
        }
    }
}