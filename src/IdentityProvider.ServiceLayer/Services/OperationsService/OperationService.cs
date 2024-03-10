
using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Module.CrossCutting.Models.ViewModels.Operations;
using System.Linq.Expressions;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace IdentityProvider.ServiceLayer.Services.OperationsService
{
    public class OperationService : Service<Operation>, IOperationService
    {
        #region Private props

        private readonly ITrackableRepository<Operation> _repository;

        #endregion Private props

        #region Ctor

        public OperationService(ITrackableRepository<Operation> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion Ctor

        #region CRUD

        public IList<OperationsDatatableSearchClass> GetDataFromDbase(
         int userId, string searchBy, int take, int skip,
         string sortBy, bool sortDir, DateTime? from, DateTime? to,
         bool alsoActive, bool alsoDeleted, out int filteredResultsCount,
         out int totalResultsCount)
        {
            var whereClause = BuildDynamicWhereClause(searchBy, from, to, alsoActive, alsoDeleted);

            // Start with a base query
            var query = _repository.Queryable().Where(whereClause);

            // Apply sorting
            query = sortBy switch
            {
                "Id" => sortDir ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id),
                "Name" => sortDir ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),
                "Description" => sortDir ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description),
                "Active" => sortDir ? query.OrderBy(x => x.Active) : query.OrderByDescending(x => x.Active),
                "CreatedDate" => sortDir ? query.OrderBy(x => x.CreatedDate) : query.OrderByDescending(x => x.CreatedDate),
                "ModifiedDate" => sortDir ? query.OrderBy(x => x.ModifiedDate) : query.OrderByDescending(x => x.ModifiedDate),
                _ => query.OrderBy(x => x.Id) // Default sorting
            };

            // Project to DTO and paginate
            var result = query
                .Select(m => new OperationsDatatableSearchClass
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Active = m.Active,
                    Deleted = m.IsDeleted,
                    CreatedDate = m.CreatedDate,
                    ModifiedDate = m.ModifiedDate,
                    Actions = string.Empty // TODO: This should be set according to your UI logic
                })
                .Skip(skip)
                .Take(take)
                .ToList();

            // Counts
            filteredResultsCount = query.Count(); // Apply the same whereClause but without pagination
            totalResultsCount = _repository.Queryable().Count();

            return result;
        }


        private Expression<Func<Operation, bool>> BuildDynamicWhereClause(
            string searchValue, DateTime? from, DateTime? to,
            bool alsoInActive, bool alsoDeleted)
        {
            var predicate = PredicateBuilder.New<Operation>(true);

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                predicate = predicate.And(s => EF.Functions.Like(s.Name.ToLower(), $"%{searchValue.ToLower()}%")
                    || EF.Functions.Like(s.Description.ToLower(), $"%{searchValue.ToLower()}%"));
            }

            if (from.HasValue && to.HasValue)
            {
                predicate = predicate.And(s => s.ModifiedDate <= to && s.ModifiedDate >= from);
            }

            // Adjust the logic here based on actual requirements
            predicate = predicate.And(s => (!alsoInActive || s.Active) && (!alsoDeleted || s.IsDeleted));

            return predicate;
        }

        #endregion CRUD

    }
}