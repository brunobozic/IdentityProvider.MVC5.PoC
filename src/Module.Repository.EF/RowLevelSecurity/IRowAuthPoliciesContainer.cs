using System;
using System.Linq.Expressions;

namespace Module.Repository.EF.RowLevelSecurity
{
    public interface IRowAuthPoliciesContainer
    {
        bool HasPolicy<TEntity>();
        IRowAuthPolicy<TEntity> GetPolicy<TEntity>();
        RowAuthPolicy<TEntity, TProperty> Register<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> selector);
    }
}
