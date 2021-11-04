using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Abp.Linq.Expressions;

using IdentityProvider.Models.ViewModels;
using LinqKit;
using Module.Repository.EF.Repositories;
using Module.ServicePattern;
using StructureMap;
using PredicateBuilder = LinqKit.PredicateBuilder;

namespace IdentityProvider.Services.AuditTrailService
{
    public class AuditTrailService : Service<DbAuditTrail>, IAuditTrailService
    {
        [DefaultConstructor] // Set Default Constructor for StructureMap
        public AuditTrailService(IRepositoryAsync<DbAuditTrail> repository) : base(repository)
        {
        }

        public List<YourCustomSearchClass> GetDataFromDbase(
            int userId
            , string searchBy
            , string searchValueExtra
            , string searchValueUserName
            , string searchValueOldValue
            , string searchValueNewValue
            , List<string> searchByTableNames
            , List<string> searchByActionNames
            , int take
            , int skip
            , string sortBy
            , bool sortDir
            , out int filteredResultsCount
            , out int totalResultsCount
        )
        {
            var whereClause = BuildDynamicWhereClause(
                searchBy
                , searchValueExtra
                , searchValueUserName
                , searchValueOldValue
                , searchValueNewValue
                , searchByTableNames
                , searchByActionNames
            );

            if (string.IsNullOrEmpty(searchBy))
            {
                // if we have an empty search then just order the results by Id ascending
                // sortBy = "Id";
                // sortDir = true;
            }

            var query =
                Queryable()
                    .Where(whereClause)
                    .Select(m => new YourCustomSearchClass
                    {
                        Id = m.Id,
                        NewData =
                            m.NewData, // GDPR => need to check whether the user has the privileges to see the raw data, else mask the data and provide means to audit log explicit read requests
                        OldData =
                            m.OldData, // GDPR => need to check whether the user has the privileges to see the raw data, else mask the data and provide means to audit log explicit read requests
                        TableName = m.TableName,
                        UserName = m
                            .UserName, // GDPR => need to check whether the user has the privileges to see the raw data, else mask the data and provide means to audit log explicit read requests
                        Action = m.Actions,
                        TableId = m.TableIdValue,
                        UpdatedAt = m.UpdatedAt
                    });

            switch (sortBy)
            {
                case "Id":

                    query = sortDir ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);

                    break;

                case "NewData":

                    query = sortDir ? query.OrderBy(x => x.NewData) : query.OrderByDescending(x => x.NewData);

                    break;

                case "OldData":

                    query = sortDir ? query.OrderBy(x => x.OldData) : query.OrderByDescending(x => x.OldData);

                    break;

                case "TableName":

                    query = sortDir ? query.OrderBy(x => x.TableName) : query.OrderByDescending(x => x.TableName);

                    break;

                case "UserName":

                    query = sortDir ? query.OrderBy(x => x.UserName) : query.OrderByDescending(x => x.UserName);

                    break;

                case "Actions":

                    query = sortDir ? query.OrderBy(x => x.Action) : query.OrderByDescending(x => x.Action);

                    break;

                case "TableIdValue":

                    query = sortDir ? query.OrderBy(x => x.TableId) : query.OrderByDescending(x => x.TableId);

                    break;

                case "UpdatedAt":

                    query = sortDir ? query.OrderBy(x => x.UpdatedAt) : query.OrderByDescending(x => x.UpdatedAt);

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

        private Expression<Func<DbAuditTrail, bool>> BuildDynamicWhereClause(
            string searchValue
            , string searchValueExtra
            , string searchValueUserName
            , string searchValueOldValue
            , string searchValueNewValue
            , List<string> searchByTableNames
            , List<string> searchByActionNames
        )
        {
            // simple method to dynamically plugin a where clause
            var predicate = PredicateBuilder.New<DbAuditTrail>(true); // true -where(true) return all


            if (string.IsNullOrWhiteSpace(searchValue) == false)
            {
                var searchTerms = searchValue.Split(' ').ToList().ConvertAll(x => x.ToLower());
                predicate = predicate.Or(s => searchTerms.Any(srch => s.TableName.ToLower().Contains(srch)));
            }

            if (string.IsNullOrWhiteSpace(searchValueExtra) == false)
            {
                var searchTerms = searchValueExtra.Split(' ').ToList().ConvertAll(x => x.ToLower());
                predicate = predicate.Or(s => searchTerms.Any(srch => s.TableName.ToLower().Contains(srch)));
            }

            if (string.IsNullOrWhiteSpace(searchValueUserName) == false)
            {
                var searchTerms = searchValueUserName.Split(' ').ToList().ConvertAll(x => x.ToLower());
                predicate = predicate.Or(s => searchTerms.Any(srch => s.UserName.ToLower().Contains(srch)));
            }

            if (string.IsNullOrWhiteSpace(searchValueOldValue) == false)
            {
                var searchTerms = searchValueOldValue.Split(' ').ToList().ConvertAll(x => x.ToLower());
                predicate = predicate.Or(s => searchTerms.Any(srch => s.OldData.ToLower().Contains(srch)));
            }

            if (string.IsNullOrWhiteSpace(searchValueNewValue) == false)
            {
                var searchTerms = searchValueNewValue.Split(' ').ToList().ConvertAll(x => x.ToLower());
                predicate = predicate.Or(s => searchTerms.Any(srch => s.NewData.ToLower().Contains(srch)));
            }

            if (searchByTableNames != null && searchByTableNames.Any() && searchByActionNames == null)
                predicate = predicate.Or(s =>
                    searchByTableNames.Any(srch => s.TableName.ToLower().Contains(srch.ToLower())));

            if (searchByActionNames != null && searchByActionNames.Any() && searchByTableNames == null)
                predicate = predicate.Or(s =>
                    searchByActionNames.Any(srch => s.Actions.ToLower().Contains(srch.ToLower())));

            if (searchByActionNames != null && searchByTableNames != null && searchByActionNames.Any() &&
                searchByTableNames.Any())
            {
                predicate = predicate.And(s =>
                    searchByActionNames.Any(srch => s.Actions.ToLower().Contains(srch.ToLower())));
                predicate = predicate.And(s =>
                    searchByTableNames.Any(srch => s.TableName.ToLower().Contains(srch.ToLower())));
            }

            return predicate;
        }
    }
}