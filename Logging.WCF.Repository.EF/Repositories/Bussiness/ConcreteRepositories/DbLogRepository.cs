using Logging.WCF.Repository.EF.EFDataContext;
using System;
using Logging.WCF.Repository.EF.RepositoryBaseImpl;
using Logging.WCF.Repository.EF.RepositoryBaseImpl.RepositoryBaseInterfaces;

namespace Logging.WCF.Repository.EF.Repositories.Bussiness.ConcreteRepositories
{
    public class DbLogRepository : EntityBaseRepositoryAsync<DatabaseLog>, IEntityBaseRepositoryAsync<DatabaseLog>
    {
        public DbLogRepository(MyAppDbContext context) : base(context)
        {

        }
    }
}
