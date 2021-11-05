using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using IdentityProvider.Infrastructure;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Models.Datatables;
using IdentityProvider.Models.ViewModels;
using IdentityProvider.Services.AuditTrailService;
using StructureMap;

namespace IdentityProvider.Controllers.Controllers
{
    public class AuditTrailController : BaseController
    {
        private readonly IAuditTrailService _auditTrailService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        [DefaultConstructor] // Set Default Constructor for StructureMap
        public AuditTrailController(
            IAuditTrailService auditTrailService
            , ICookieStorageService cookieStorageService
            , IErrorLogService errorLogService
            , IUnitOfWorkAsync unitOfWorkAsync
        ) : base(cookieStorageService
            , errorLogService
        )
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _auditTrailService = auditTrailService;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AuditTrailDatatables(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var res = SearchFunction(model, out var filteredResultsCount, out var totalResultsCount);

            var result = new List<YourCustomSearchClass>(res.Count);

            result.AddRange(res.Select(s => new YourCustomSearchClass
            {
                Id = s.Id,
                UpdatedAt = s.UpdatedAt,
                UserId = s.UserId,
                Action = s.Action,
                TableName = s.TableName,
                OldData = s.OldData,
                NewData = s.NewData,
                TableId = s.TableId,
                Tables = s.Tables,
                Actions = s.Actions
            }));

            return Json(new
            {
                // this is what datatables expect to recieve
                model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            });
        }

        private IList<YourCustomSearchClass> SearchFunction(
            DataTableAjaxPostModel model
            , out int filteredResultsCount
            , out int totalResultsCount
        )
        {
            var searchBy = model.search?.value;
            var take = model.length;
            var skip = model.start;
            var userId = model.userid;

            var sortBy = string.Empty;
            var sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            if (searchBy == null)
            {
                // if any of the columns have server-side search set on them (not all of them should!)
                // use the search string provided with the column to perform server side searching.
            }

            var searchByExtra = model.search_extra;
            var searchByUserName = model.search_userName;
            var searchByOldValue = model.search_oldValue;
            var searchByNewValue = model.search_newValue;
            var searchByTableNames = model.tables;
            var searchByActionNames = model.actions;
            // search the dbase taking into consideration table sorting and paging
            var result = _auditTrailService.GetDataFromDbase(
                userId
                , searchBy
                , searchByExtra
                , searchByUserName
                , searchByOldValue
                , searchByNewValue
                , searchByTableNames
                , searchByActionNames
                , take
                , skip
                , sortBy
                , sortDir
                , out filteredResultsCount
                , out totalResultsCount
            );


            if (result == null)
                // empty collection...
                return new List<YourCustomSearchClass>();

            return result;
        }

        private MultiSelectList GetMultiSelectListDropDownForTableName(List<int> selectedValues)
        {
            List<SelectListItem> items;

            if (selectedValues != null)
                items = _auditTrailService.Queryable().OrderBy(a => a.TableName).AsNoTracking().Select(c =>
                    new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.TableName,
                        Selected = selectedValues.Contains(c.Id),
                        Disabled = selectedValues
                            .Contains(c
                                .Id) // this will make the dropdown checkboxes disabled for all those items that already exist in the db, to prevent deletion
                    }).ToList();
            else
                items = _auditTrailService.Queryable().OrderBy(a => a.TableName).AsNoTracking().Select(c =>
                    new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.TableName,
                        Selected = false
                    }).ToList();

            var msl = new MultiSelectList(items, "Value", "Text", selectedValues, selectedValues);

            return msl;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetTableNameMultiselectDropdown(string search)
        {
            List<Select2Model> items = null;

            var distinctTableNames = _auditTrailService
                .Queryable()
                .GroupBy(par => par.TableName, (key, g) => g.OrderBy(e => e.TableName)
                    .FirstOrDefault())
                .ToList();

            if (search != null)
                items = distinctTableNames
                    .Where(x => x.TableName.Contains(search))
                    .Select(c => new Select2Model
                    {
                        value = c.Id.ToString(),
                        text = c.TableName,
                        selected = false,
                        id = c.Id
                    })
                    .ToList();
            else
                items = distinctTableNames
                    .Select(c => new Select2Model
                    {
                        value = c.Id.ToString(),
                        text = c.TableName,
                        selected = false,
                        id = c.Id
                    }).ToList();
            var modifiedData = items.Select(x => new
            {
                id = x.text, x.text
            });
            return Json(modifiedData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetUserNameMultiselectDropdown()
        {
            return null;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetActionMultiselectDropdown()
        {
            return null;
        }
    }
}