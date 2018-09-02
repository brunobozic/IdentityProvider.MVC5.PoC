using System.Data.Entity;
using IdentityProvider.Infrastructure.Domain;
using Module.Repository.EF.Repositories;
using Module.Repository.EF.RowLevelSecurity;
using Module.Repository.EF.UnitOfWorkInterfaces;
using TrackableEntities;

namespace Module.Repository.EF.IntegrationTests.UnitTests.Fake
{
    public class FakeRepository<TEntity> : Repository<TEntity> where TEntity : DomainEntity<int>
    {
        public FakeRepository( DbContext context , IUnitOfWorkAsync unitOfWork , IRowAuthPoliciesContainer rowAuthPoliciesContainer )
            : base(context , unitOfWork, rowAuthPoliciesContainer)
        {
        }

        public override void Delete( TEntity entity )
        {
            entity.TrackingState = TrackingState.Deleted;
            Set.Attach(entity);
        }
    }
}