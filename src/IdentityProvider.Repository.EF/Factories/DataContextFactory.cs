using IdentityProvider.Infrastructure.SessionStorageFactories;
using IdentityProvider.Repository.EF.EFDataContext;

namespace IdentityProvider.Repository.EF.Factories
{
    public static class DataContextFactory
    {
        /// <summary>
        /// </summary>
        public static void ClearDataContext()
        {
            var dataContextStorageContainer =
                DataContextStorageFactory<AppDbContext>.CreateStorageContainer();
            dataContextStorageContainer.Clear();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static AppDbContext GetDataContextAsync()
        {
            var dataContextStorageContainer =
                DataContextStorageFactory<AppDbContext>.CreateStorageContainer();

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