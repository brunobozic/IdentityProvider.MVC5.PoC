
using IdentityProvider.Repository.EFCore.Domain;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Module.CrossCutting.Models.ViewModels;
using System.Linq.Expressions;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace IdentityProvider.ServiceLayer.Services.AuditTrailService
{
    public class AuditTrailService : Service<DbAuditTrail>, IAuditTrailService
    {
        #region Private Props

        private readonly ITrackableRepository<DbAuditTrail> _repository;

        #endregion Private Props

        #region Ctor

        public AuditTrailService(ITrackableRepository<DbAuditTrail> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion Ctor

        #region CRUD

        public List<YourCustomSearchClass> GetDataFromDbase(
            int userId, string searchBy, string searchValueExtra, string searchValueUserName,
            string searchValueOldValue, string searchValueNewValue, List<string> searchByTableNames,
            List<string> searchByActionNames, int take, int skip, string sortBy, bool sortDir,
            out int filteredResultsCount, out int totalResultsCount)
        {
            var whereClause = BuildDynamicWhereClause(
                searchBy, searchValueExtra, searchValueUserName, searchValueOldValue, searchValueNewValue,
                searchByTableNames, searchByActionNames);

            var query = _repository.Queryable()
                .Where(whereClause)
                .Select(m => new YourCustomSearchClass
                {
                    Id = m.Id,
                    NewData = m.NewData,
                    OldData = m.OldData,
                    TableName = m.TableName,
                    UserName = m.UserName,
                    Action = m.Actions,
                    TableId = m.TableIdValue,
                    UpdatedAt = m.UpdatedAt
                });

            query = ApplySorting(query, sortBy, sortDir);

            var result = query.Skip(skip).Take(take).ToList();

            filteredResultsCount = _repository.Queryable().Where(whereClause).Count();
            totalResultsCount = _repository.Queryable().Count();

            return result;
        }

        private static IQueryable<YourCustomSearchClass> ApplySorting(IQueryable<YourCustomSearchClass> query, string sortBy, bool sortDir)
        {
            return sortBy switch
            {
                "Id" => sortDir ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id),
                "NewData" => sortDir ? query.OrderBy(x => x.NewData) : query.OrderByDescending(x => x.NewData),
                "OldData" => sortDir ? query.OrderBy(x => x.OldData) : query.OrderByDescending(x => x.OldData),
                "TableName" => sortDir ? query.OrderBy(x => x.TableName) : query.OrderByDescending(x => x.TableName),
                "UserName" => sortDir ? query.OrderBy(x => x.UserName) : query.OrderByDescending(x => x.UserName),
                "Actions" => sortDir ? query.OrderBy(x => x.Action) : query.OrderByDescending(x => x.Action),
                "TableIdValue" => sortDir ? query.OrderBy(x => x.TableId) : query.OrderByDescending(x => x.TableId),
                "UpdatedAt" => sortDir ? query.OrderBy(x => x.UpdatedAt) : query.OrderByDescending(x => x.UpdatedAt),
                _ => query.OrderBy(x => x.Id),
            };
        }

        private Expression<Func<DbAuditTrail, bool>> BuildDynamicWhereClause(
            string searchValue, string searchValueExtra, string searchValueUserName, string searchValueOldValue,
            string searchValueNewValue, List<string> searchByTableNames, List<string> searchByActionNames)
        {
            var predicate = PredicateBuilder.New<DbAuditTrail>(true);

            AddSearchTermPredicate(ref predicate, searchValue, nameof(DbAuditTrail.TableName));
            AddSearchTermPredicate(ref predicate, searchValueExtra, nameof(DbAuditTrail.TableName));
            AddSearchTermPredicate(ref predicate, searchValueUserName, nameof(DbAuditTrail.UserName));
            AddSearchTermPredicate(ref predicate, searchValueOldValue, nameof(DbAuditTrail.OldData));
            AddSearchTermPredicate(ref predicate, searchValueNewValue, nameof(DbAuditTrail.NewData));

            if (searchByTableNames?.Any() == true)
            {
                predicate = predicate.And(s => searchByTableNames.Contains(s.TableName));
            }

            if (searchByActionNames?.Any() == true)
            {
                predicate = predicate.And(s => searchByActionNames.Contains(s.Actions));
            }

            return predicate;
        }

        private static void AddSearchTermPredicate(ref ExpressionStarter<DbAuditTrail> predicate, string searchTerm, string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTerms = searchTerm.Split(' ').Select(x => x.ToLower()).ToList();
                predicate = predicate.Or(s => searchTerms.Any(term => EF.Functions.Like(EF.Property<string>(s, propertyName).ToLower(), $"%{term}%")));
            }
        }

        #endregion CRUD
    }

}