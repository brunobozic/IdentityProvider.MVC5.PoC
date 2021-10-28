using System;
using System.Linq.Expressions;

namespace Module.Repository.EF.RowLevelSecurity
{
    public class RowAuthPolicy<TEntity, TProperty> : IRowAuthPolicy<TEntity>
    {
        private readonly IRowAuthPoliciesContainer _parent;
        private readonly Expression<Func<TEntity, TProperty>> _selector;
        private Func<TProperty> _filterValueGetter;
        private Func<string> _isEqualTo;
        private Func<bool> _shouldApplyFunc;

        // container.Register<Operation , string>(o => o.Name).When(GetWhenCriteria).Match(GetMatchingCriteria);
        public RowAuthPolicy(Expression<Func<TEntity, TProperty>> selector, IRowAuthPoliciesContainer parent)
        {
            _selector = selector;
            _parent = parent;
            _filterValueGetter = () => default;
            EntityType = typeof(TEntity);
        }

        public Type EntityType { get; }

        public Expression<Func<TEntity, bool>> BuildAuthFilterExpression()
        {
            if (_shouldApplyFunc.Invoke()) return BuildFilerExpression();

            return entity => true;
        }

        private Expression<Func<TEntity, bool>> BuildFilerExpression()
        {
            var value = _filterValueGetter.Invoke();
            Expression<Func<TProperty>> filterValueParam = () => value;

            var filterExpression = Expression.Lambda<Func<TEntity, bool>>(
                Expression.MakeBinary(ExpressionType.Equal,
                    Expression.Convert(_selector.Body, typeof(TProperty)),
                    filterValueParam.Body),
                _selector.Parameters);

            return filterExpression;
        }


        public IRowAuthPoliciesContainer Match(Func<TProperty> filterValueGetFunc)
        {
            _filterValueGetter = filterValueGetFunc;

            return _parent;
        }

        public RowAuthPolicy<TEntity, TProperty> When(Func<bool> condition)
        {
            _shouldApplyFunc = condition;

            return this;
        }
    }
}