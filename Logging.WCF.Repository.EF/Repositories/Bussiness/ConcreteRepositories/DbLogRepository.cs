using Logging.WCF.Repository.EF.EFDataContext;
using Logging.WCF.Repository.EF.Repositories.DomainCore.RepositoryBaseImpl;
using Logging.WCF.Repository.EF.Repositories.DomainCore.RepositoryBaseImpl.RepositoryBaseInterfaces;
using System;

namespace Logging.WCF.Repository.EF.Repositories.Bussiness.ConcreteRepositories
{
    public class DbLogRepository : EntityBaseRepositoryAsync<DatabaseLog>, IEntityBaseRepositoryAsync<DatabaseLog>
    {
        public DbLogRepository(MyAppDbContext context) : base(context)
        {

        }
    }
}
