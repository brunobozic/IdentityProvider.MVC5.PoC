using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using IdentityProvider.Models;
using IdentityProvider.Models.Domain.Account;
using LinqKit;
using Module.Repository.EF.Repositories;
using Module.ServicePattern;


namespace IdentityProvider.Services.OperationsService
{
    public class OperationsService : Service<Operation>, IOperationService
    {
        [StructureMap.DefaultConstructor] // Set Default Constructor for StructureMap
        public OperationsService(IRepositoryAsync<Operation> repository) : base(repository)
        {

        }

        private Expression<Func<Operation, bool>> BuildDynamicWhereClause(
            string searchValue
            , DateTime? from
            , DateTime? to
            , bool alsoActive = false
            , bool alsoDeleted = false
            )
        {
            // simple method to dynamically plugin a where clause
            var predicate = PredicateBuilder.New<Operation>(true); // true -where(true) return all

            if (string.IsNullOrWhiteSpace(searchValue) == false)
            {
                var searchTerms = searchValue.Split(' ').ToList().ConvertAll(x => x.ToLower());
                predicate = predicate.Or(s => searchTerms.Any(srch => s.Description.ToLower().Contains(srch)));
                predicate = predicate.Or(s => searchTerms.Any(srch => s.Name.ToLower().Contains(srch)));
            }

            if (from.HasValue && to.HasValue)
            {
                predicate = predicate.And(s => s.ModifiedDate <= to && s.ModifiedDate >= from);
            }

            if (alsoActive)
                predicate = predicate.And(s => s.Active == alsoActive);

            if (alsoDeleted)
                predicate = predicate.And(s => s.IsDeleted == alsoDeleted);

            return predicate;
        }

        public IList<OperationsDatatableSearchClass> GetDataFromDbase(
            int userId
            , string searchBy
            , int take
            , int skip
            , string sortBy
            , bool sortDir
            , DateTime? from
            , DateTime? to
            , bool also_active
            , bool also_deleted
            , out int filteredResultsCount
            , out int totalResultsCount
            )
        {
            var whereClause = BuildDynamicWhereClause(searchBy, from, to, also_active, also_deleted);

            if (string.IsNullOrEmpty(searchBy))
            {
                // if we have an empty search then just order the results by Id ascending
                // sortBy = "Id";
                // sortDir = true;
            }

            var query = Queryable()
                        .Where(whereClause)
                        .Select(m => new OperationsDatatableSearchClass
                        {
                            Id = m.Id,
                            Name = m.Name,
                            Description = m.Description,
                            Active = m.Active,
                            CreatedDate = m.CreatedDate,
                            ModifiedDate = m.ModifiedDate,
                            Actions = ""
                        });

            switch (sortBy)
            {
                case "Id":
                    query = sortDir ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                    break;

                case "Name":
                    query = sortDir ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                    break;

                case "Description":
                    query = sortDir ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
                    break;

                case "Active":
                    query = sortDir ? query.OrderBy(x => x.Active) : query.OrderByDescending(x => x.Active);
                    break;

                case "CreatedDate":
                    query = sortDir ? query.OrderBy(x => x.CreatedDate) : query.OrderByDescending(x => x.CreatedDate);
                    break;

                case "ModifiedDate":
                    query = sortDir ? query.OrderBy(x => x.ModifiedDate) : query.OrderByDescending(x => x.ModifiedDate);
                    break;

                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            var result = query
                .Skip(skip)
                .Take(take)
                .ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            // "Entity Framework's query processing pipeline cannot handle invocation expressions, which is why you need to call AsExpandable on the first object in the query. 
            // By calling AsExpandable, you activate LINQKit's expression visitor class which substitutes invocation expressions with simpler constructs that Entity Framework can understand."
            // ~Josef Albahary
            filteredResultsCount = _repository
                .Queryable()
                .AsExpandable()
                .Where(whereClause)
                .Count();

            totalResultsCount = _repository
                .Queryable()
                .Count();

            return result;
        }
    }
}