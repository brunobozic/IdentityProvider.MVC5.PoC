using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Models;
using IdentityProvider.Models.Datatables;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Models.ViewModels;
using IdentityProvider.Models.ViewModels.Operations;
using IdentityProvider.Models.ViewModels.Operations.Extensions;
using IdentityProvider.Services.OperationsService;
using Module.Repository.EF.UnitOfWorkInterfaces;
using StructureMap;
using TrackableEntities;

namespace IdentityProvider.Controllers.Controllers
{
    public class OperationController : BaseController, IController
    {
        private readonly IOperationService _operationService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        [DefaultConstructor]
        public OperationController(
            ICookieStorageService cookieStorageService
            , IErrorLogService errorLogService
            , IUnitOfWorkAsync unitOfWorkAsync
            , IOperationService operationService
            , IApplicationConfiguration applicationConfiguration
        )
            : base(
                cookieStorageService
                , errorLogService
                , applicationConfiguration
            )
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _operationService = operationService;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult OperationsGetAllPaged(
     )
        {

            var returnValue = new OperationPagedVm
            {
                HeaderRightSideActionDropdownList = new List<HeaderRightSideActionDropdown>()
       
            };

            returnValue.HeaderRightSideActionDropdownList.Add(new HeaderRightSideActionDropdown
            {
                WidgetId = "OperationsWidget",
                ViewName = "OperationsDashboard"
            });

            return View(returnValue);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult OperationsGetAll(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var res = SearchFunction(model, out var filteredResultsCount, out var totalResultsCount);
      
            var result = new List<OperationsDatatableSearchClass>(res.Count);

            result.AddRange(res.Select(s => new OperationsDatatableSearchClass
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Active = s.Active,
                CreatedDate = s.CreatedDate,
                ModifiedDate = s.ModifiedDate,
                Actions = s.Actions
            }));

            return Json(new
            {
                // this is what datatables expect to recieve
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            });
        }

        private IList<OperationsDatatableSearchClass> SearchFunction(
            DataTableAjaxPostModel model
            , out int filteredResultsCount
            , out int totalResultsCount
            )
        {
            var searchBy = model.search?.value ?? model.search_extra;
            var take = model.length;
            var skip = model.start;
            var userId = model.userid;
            var from = model.from;
            var to = model.to;
            var alsoactive = model.alsoactive;
            var alsodeleted = model.alsodeleted;
            var sortBy = "";
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

            // search the dbase taking into consideration table sorting and paging
            var result = _operationService.GetDataFromDbase(
                userId
                , searchBy
                , take
                , skip
                , sortBy
                , sortDir
                , from
                , to
                , alsoactive
                , alsodeleted
                , out filteredResultsCount
                , out totalResultsCount
            );

            if (result == null)
            {
                // empty collection...
                return new List<OperationsDatatableSearchClass>();
            }

            return result;
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<JsonResult> FetchInfoOnOperations()
        {
            var retVal = new InfoOnOperationsVm
            {
                ActiveItemCount = 0,
                DeletedItemCount = 0,
                InactiveItemCount = 0,
                Success = false,
                Message = string.Empty
            };

            try
            {
                var queryResult = await _unitOfWorkAsync.RepositoryAsync<Operation>().Queryable().AsNoTracking().Select(i =>
                    new OperationCountsDto
                    {
                        Name = i.Name,
                        Active = i.Active,
                        Deleted = i.IsDeleted
                    }).ToListAsync();

                retVal.ActiveItemCount = queryResult.Count(op => op.Active && !op.Deleted);
                retVal.DeletedItemCount = queryResult.Count(op => op.Deleted); // might conflict with row based access security
                retVal.InactiveItemCount = queryResult.Count(op => !op.Active);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
                _errorLogService.LogError(this, e.Message, e);
                retVal.Message = e.Message ?? "";
            }

            retVal.Success = true;

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Insert()
        {
            return PartialView("Partial/_operationInsertPartial");
        }

        // POST: /Operation/Insert/5
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> Insert(OperationToInsertVm operationToInsert)
        {
            var retVal = new OperationInsertedVm { Success = false };

            if (!ModelState.IsValid)
            {
                if (ModelState.Values.Any(i => i.Errors.Count > 0))
                {
                    var problems = ModelState.Values.Where(i => i.Errors.Count > 0).ToList();
                }

                retVal.Message = "Model state invalid";
                retVal.FormErrors = ModelState.Select(kvp => new { key = kvp.Key, errors = kvp.Value.Errors.Select(e => e.ErrorMessage) });
            }

            var op = new Operation
            {
                Name = operationToInsert.Name,
                Description = operationToInsert.Description,
                Active = operationToInsert.MakeActive,
                ActiveFrom = DateTime.Now,
                ActiveTo = operationToInsert.ActiveUntil
            };

            var validationResults = op.Validate();

            if (validationResults != null && validationResults.Any())
            {
                retVal.Success = false;

                var sb = new StringBuilder();

                foreach (var validation in validationResults)
                {
                    sb.Append(validation.ErrorMessage);
                }

                ModelState.AddModelError("Name", sb.ToString());
                retVal.ValidationIssues = sb.ToString();

                return Json(retVal, JsonRequestBehavior.AllowGet);
            }

            var inserted = -1;

            try
            {
                _unitOfWorkAsync.RepositoryAsync<Operation>().Insert(op);
                inserted = await _unitOfWorkAsync.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
                _errorLogService.LogError(this, e.Message, e);
                retVal.Message = e.Message ?? "";
            }

            retVal.WasInserted = inserted;
            retVal.Success = true;

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        // POST: /Operation/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> Delete(string itemToDelete)
        {
            var retVal = new OperationDeletedVm { WasDeleted = false };

            if (string.IsNullOrEmpty(itemToDelete))
            {
                throw new ArgumentNullException(nameof(itemToDelete));
            }

            try
            {
                var repo = _unitOfWorkAsync.RepositoryAsync<Operation>();
                await repo.DeleteAsync(int.Parse(itemToDelete));
                var result = await _unitOfWorkAsync.SaveChangesAsync();

                if (result > 0)
                    retVal.WasDeleted = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
                _errorLogService.LogError(this, e.Message, e);
                retVal.Error = e.Message ?? "";
            }

            return Json(retVal.WasDeleted, JsonRequestBehavior.AllowGet);
        }

        // GET: /Operation/Edit/5
        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult Edit(int id)
        {
            var retVal = new OperationVm
            {
                Success = false,
                Message = ""
            };

            if (id <= 0)
            {
                retVal.Message = "Please provide an id.";
                return PartialView("Partial/_operationEditPartial", retVal);
            }

            var result = _operationService.Find(id);

            if (result != null)
            {
                retVal.Success = true;
                retVal.Operation = result.ConvertToViewModel();
            }
            else
            {
                retVal.Message = "Item with requested Id was not found.";
            }

            return PartialView("Partial/_operationEditPartial", retVal);

        }

        // POST: /Operation/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, Name, Description, Active, ActiveFrom, ActiveTo, RowVersion")] Operation operation)
        {
            // https://stackoverflow.com/questions/39533599/mvc-5-with-bootstrap-modal-from-partial-view-validation-not-working
            // https://stackoverflow.com/questions/2845852/asp-net-mvc-how-to-convert-modelstate-errors-to-json
            var retVal = new OperationVm
            {
                Success = false,
                Message = ""
            };

            if (ModelState.IsValid)
            {
                var repo = _unitOfWorkAsync.RepositoryAsync<Operation>();
                var valid = operation.Validate();

                try
                {
                    var existingEntity = await repo.FindAsync(operation.Id);

                    if (existingEntity == null)
                    {
                        ModelState.AddModelError(string.Empty, @"Unable to update entity. The entity was deleted by another user.");
                    }

                    existingEntity.Name = operation.Name;
                    existingEntity.Description = operation.Description;

                    if (existingEntity.Active != operation.Active)
                    {
                        // the item has been deactivated...
                        if (existingEntity.Active && !operation.Active)
                        {
                            // set the date of deactivation to current date, dont touch the created date
                            existingEntity.ActiveTo = DateTime.Now;
                        }

                        // the item has been reactivated...
                        if (!existingEntity.Active && operation.Active)
                        {
                            // audit log?
                            existingEntity.Active = true;
                            existingEntity.ActiveFrom = DateTime.Now;
                            existingEntity.ActiveTo = operation.ActiveTo;
                        }
                    }
                    else
                    {
                        // extending the activation validity date...
                        existingEntity.ActiveTo = operation.ActiveTo;
                    }

                    existingEntity.Active = operation.Active;

                    // you are not allowed to change the value of active from, you naughty naughty person!
                    existingEntity.TrackingState = TrackingState.Modified;
                    existingEntity.RowVersion = operation.RowVersion;

                    // Warning: concurrency issues (row versions) must be resolved here...
                    repo.Update(existingEntity);

                    var result = await _unitOfWorkAsync.SaveChangesAsync();

                    if (result > 0)
                    {
                        retVal.Success = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Debug.WriteLine(e);
                    retVal.Message = e.Message;

                    // if we stumbled upon an optimistic concurrency error, emit proper error to the user, and have the view reloaded with the new values taken from the database
                    if (e.Message.Contains("The record you attempted to edit was modified by another user after you got the original value. The edit operation was canceled and the current values in the database have been displayed."))
                    {
                        return Json(new { Success = false, OptimisticConcurrencyError = true, OptimisticConcurrencyErrorMsg = "The record you attempted to edit was modified by another user after you got the original value. The edit operation was canceled and the current values in the database have been displayed." });
                    }
                }
            }
            else
            {
                // the model aint valid, we need to return the user to the original view he used to enter data for him to fix the entry...
                retVal.Success = false;

                var errorModel =
                    from x in ModelState.Keys
                    where ModelState[x].Errors.Count > 0
                    select new
                    {
                        key = x.First().ToString().ToUpper() + string.Join("", x.Skip(1)),
                        errors = ModelState[x].Errors.
                            Select(y => y.ErrorMessage).
                            ToArray()
                    };

                return Json(new { Success = false, FormErrors = errorModel });
            }

            return Json(new { Success = true });
        }

        // POST: /Operation/Detail/5
        // GET: /Operation/Detail/5
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<PartialViewResult> Detail(int id)
        {
            var retVal = new OperationVm
            {
                Success = false,
                Message = ""
            };

            if (id <= 0)
            {
                retVal.Message = "Please provide an id.";
                return PartialView("Partial/_operationDetailsPartial", retVal);
            }

            var result = await _operationService.FindAsync(id);

            if (result != null)
            {
                retVal.Success = true;

                if (Request.IsAjaxRequest())
                {
                    retVal.Operation = result.ConvertToViewModel();
                }
            }
            else
            {
                retVal.Message = "Item with requested Id was not found.";
            }

            return PartialView("Partial/_operationDetailsPartial", retVal);
        }
    }
}

