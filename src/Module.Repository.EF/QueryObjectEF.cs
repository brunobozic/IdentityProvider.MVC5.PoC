using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IdentityProvider.Models.Domain.Account;
using Module.Repository.EF.SimpleAudit;

namespace Module.Repository.EF
{
    public abstract class QueryObjectEf<TEntity>
    {
        protected readonly IAuditedDbContext<ApplicationUser> Context;

        protected QueryObjectEf(IAuditedDbContext<ApplicationUser> context)
        {
            Context = context;
        }

        public IEnumerable<TEntity> Execute()
        {
            return DoExecute().ToList();
        }

        protected abstract IEnumerable<TEntity> DoExecute();
    }
}