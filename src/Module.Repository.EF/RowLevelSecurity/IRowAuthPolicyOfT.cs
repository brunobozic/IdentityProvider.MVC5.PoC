using System;
using System.Linq.Expressions;

namespace Module.Repository.EF.RowLevelSecurity
{
    public interface IRowAuthPolicy<TEntity>
    {
        Type EntityType { get; }
        Expression<Func<TEntity, bool>> BuildAuthFilterExpression();
    }
}
