using IdentityProvider.Infrastructure.SessionStorageFactories;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Repository.EF.EFDataContext;
using Module.Repository.EF.SimpleAudit;

namespace IdentityProvider.Repository.EF.Factories
{
    public static class DataContextFactory
    {
        /// <summary>
        /// </summary>
        public static void ClearDataContext()
        {
            var dataContextStorageContainer =
                DataContextStorageFactory<IAuditedDbContext<ApplicationUser>>.CreateStorageContainer();
            dataContextStorageContainer.Clear();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static IAuditedDbContext<ApplicationUser> GetDataContextAsync()
        {
            var dataContextStorageContainer =
                DataContextStorageFactory<IAuditedDbContext<ApplicationUser>>.CreateStorageContainer();

            var contactManagerContext = dataContextStorageContainer.GetDataContext();

            if (contactManagerContext == null)
            {
                contactManagerContext = new AppDbContext("SimpleMembership");
                dataContextStorageContainer.Store(contactManagerContext);
            }
            return contactManagerContext;
        }
    }
}