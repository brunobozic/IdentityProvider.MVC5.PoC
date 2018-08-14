using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using IdentityProvider.Infrastructure.DatabaseAudit;
using IdentityProvider.Infrastructure.DatabaseLog.Model;
using IdentityProvider.Models.Domain.Account;
using TrackableEntities;

namespace Module.Repository.EF.SimpleAudit
{
    public interface IAuditedDbContext<T>
    {
        DbSet<DbLog> DatabaseLog { get; set; }
        DbSet<Resource> Resource { get; set; }
        DbSet<Operation> Operation { get; set; }

        DbSet<DbAuditTrail> DbAuditTrail { get; set; }
        Guid InstanceId { get; set; }
        Database Database { get; set; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Database GetDatabase();


        /// <summary>
        /// </summary>
        /// <returns></returns>
        DbChangeTracker GetChangeTracker();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        DbContextConfiguration GetConfiguration();

        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbSet<T> Set<T>() where T : class;
        void ApplyChanges(ITrackable entity);
    }
}