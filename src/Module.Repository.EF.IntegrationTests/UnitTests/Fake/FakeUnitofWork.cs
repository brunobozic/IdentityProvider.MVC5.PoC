using System;
using System.Collections.Generic;
using System.Data.Entity;
using Module.Repository.EF.Repositories;
using Module.Repository.EF.RowLevelSecurity;

namespace Module.Repository.EF.IntegrationTests.UnitTests.Fake
{
    public class FakeUnitofWork : UnitOfWork
    {
        private readonly DbContext _context;

        public FakeUnitofWork(DbContext context) : base(context , new RowAuthPoliciesContainer())
        {
            _context = context;
        }

        public override IRepositoryAsync<TEntity> RepositoryAsync<TEntity>()
        {
            if (Repositories == null)
            {
                Repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;

            if (Repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)Repositories[type];
            }

            // Add fake repository
            var repositoryType = typeof(FakeRepository<>);
            Repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context, this));
            return Repositories[type];
        }
    }
}