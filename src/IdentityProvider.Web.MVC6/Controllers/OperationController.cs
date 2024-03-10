using AutoMapper;
using IdentityProvider.Repository.EFCore;
using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using IdentityProvider.ServiceLayer.Services.OperationsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Module.CrossCutting.Cookies;
using Module.CrossCutting.Models.Datatables;
using Module.CrossCutting.Models.ViewModels;
using Module.CrossCutting.Models.ViewModels.Operations;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;


namespace IdentityProvider.Web.MVC6.Controllers
{
    [AllowAnonymous]
    [Route("api/operation")]
    [ApiController]
    [Produces("application/json")]
    public class OperationController : BaseController, IController
    {
        #region Private props

        private readonly IOperationService _operationService;
        private readonly ILogger<OperationController> _logger;

        #endregion Private props

        #region Ctor

        public OperationController(
            IMyUnitOfWork unitOfWork
            , IMapper mapper
            , IOptionsSnapshot<ApplicationSettings> configurationValues
            , IOperationService operationService
            , IMemoryCache memCache
            , IHttpContextAccessor contextAccessor
            , IConfiguration configuration
            , ICookieStorageService cookieStorageService
            , ILogger<OperationController> logger
            )
            :
            base(unitOfWork, mapper, configurationValues, memCache, contextAccessor, configuration, cookieStorageService, logger)
        {
            _operationService = operationService;
            _logger = logger;
        }

        #endregion Ctor

        #region Private props
        #endregion Private props

        #region Public Paginated get 
        [AllowAnonymous]
        [HttpGet("OperationsGetAllPaged")]
        [AcceptVerbs("Get", "Post")]
        public IActionResult OperationsGetAllPaged()
        {
            Log.Information("Accessed OperationsGetAllPaged to fetch all operations with pagination.");
            var returnValue = new OperationPagedVm
            {
                HeaderRightSideActionDropdownList = new List<HeaderRightSideActionDropdown>
                {
                    new HeaderRightSideActionDropdown
                    {
                        WidgetId = "OperationsWidget",
                        ViewName = "OperationsDashboard"
                    }
                }
            };

            return View(returnValue);
        }
        #endregion Public Paginated get

        #region Public get
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        [HttpPost("OperationsGetAll")]
        public JsonResult OperationsGetAll(DataTableAjaxPostModel model)
        {
            Log.ForContext("Action", "OperationsGetAll")
                   .Information("Fetching operations based on DataTableAjaxPostModel: {@DataTableAjaxPostModel}", model);

            var (result, filteredResultsCount, totalResultsCount) = SearchFunction(model);


            var data = result.Select(s => new OperationsDatatableSearchClass
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Active = s.Active,
                CreatedDate = s.CreatedDate,
                ModifiedDate = s.ModifiedDate,
                Actions = $"<a href=\"\" class=\"OperationsDashboard_OperationsDatatable_edit text-center\" data-id=\"{s.Id}\" custom-middle-align\">Edit</a> / <a href=\"\" class=\"OperationsDashboard_OperationsDatatable_remove\" data-id=\"{s.Id}\">Delete</a>"
            });

            Log.Information("Operations fetched successfully for DataTables. Total Records: {TotalRecords}, Filtered Records: {FilteredRecords}", totalResultsCount, filteredResultsCount);

            return Json(new
            {
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            });
        }


        private (IList<OperationsDatatableSearchClass> Result, int FilteredResultsCount, int TotalResultsCount) SearchFunction(DataTableAjaxPostModel model)
        {
            var searchBy = model.search?.value ?? model.search_extra;
            var take = model.length;
            var skip = model.start;
            var userId = model.userid;
            var from = model.from;
            var to = model.to;
            var alsoinactive = model.alsoinactive;
            var alsodeleted = model.alsodeleted;
            var sortBy = string.Empty;
            var sortDir = true;

            if (model.order != null && model.order.Any())
            {
                // Assuming model.order is not null and has at least one element.
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.Equals("asc", StringComparison.OrdinalIgnoreCase);
            }

            int filteredResultsCount, totalResultsCount;
            var result = _operationService.GetDataFromDbase(
                userId,
                searchBy,
                take,
                skip,
                sortBy,
                sortDir,
                from,
                to,
                alsoinactive,
                alsodeleted,
                out filteredResultsCount,
                out totalResultsCount
            );

            if (result == null)
            {
                // Log this situation as it might indicate an issue or an unexpected state.
                Log.Warning("SearchFunction returned null. Parameters: {@model}", model);
                return (new List<OperationsDatatableSearchClass>(), 0, 0);
            }

            var mappedResult = result.Select(domainObj => new OperationsDatatableSearchClass
            {

                Id = domainObj.Id,
                Name = domainObj.Name,
                Description = domainObj.Description,
                Active = domainObj.Active,
                CreatedDate = domainObj.CreatedDate,
                ModifiedDate = domainObj.ModifiedDate,
                // Actions would likely be handled in the frontend, but if needed, could be assembled here.
                Actions = string.Empty,
                Deleted = domainObj.Deleted
            }).ToList();

            return (mappedResult, filteredResultsCount, totalResultsCount);
        }

        [AllowAnonymous]
        [AcceptVerbs("Get")]
        [HttpGet("FetchInfoOnOperations")]
        public async Task<IActionResult> FetchInfoOnOperations()
        {
            try
            {
                Log.Information("Starting FetchInfoOnOperations to gather information on operations.");

                var queryResult = await _operationService.Queryable()
                    .AsNoTracking()
                    .Select(i => new OperationCountsDto
                    {
                        Name = i.Name,
                        Active = i.Active,
                        Deleted = i.IsDeleted
                    }).ToListAsync();

                var retVal = new InfoOnOperationsVm
                {
                    ActiveItemCount = queryResult.Count(op => op.Active && !op.Deleted),
                    DeletedItemCount = queryResult.Count(op => op.Deleted),
                    InactiveItemCount = queryResult.Count(op => !op.Active),
                    Success = true
                };

                Log.Information("FetchInfoOnOperations completed successfully.");

                return Json(retVal);
            }
            catch (Exception e)
            {
                Log.Error(e, "An error occurred while fetching information on operations.");
                return BadRequest(new { Message = e.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("Insert")]
        public IActionResult Insert()
        {
            Log.Information("Navigated to Insert view for creating a new operation.");
            return PartialView("Partial/_operationInsertPartial");
        }

        [AllowAnonymous]
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] OperationToInsertVm operationToInsertVm)
        {
            var retVal = new OperationInsertedVm { Success = false };
            Log.Information("Attempting to insert a new operation: {@OperationToInsertVm}", operationToInsertVm);
            if (!ModelState.IsValid)
            {
                Log.Warning("Model state is invalid. Errors: {@ModelStateErrors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);
            }

            try
            {
                var operation = Mapper.Map<Operation>(operationToInsertVm);
                var validationResults = operation.Validate();

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

                    return Json(retVal);
                }

                var inserted = -1;

                _operationService.Insert(operation);

                var gotSaved = await UnitOfWork.SaveChangesAsync();

                Log.Information("Operation inserted successfully. Operation ID: {OperationId}", operation.Id);

                retVal.WasInserted = inserted;
                retVal.Success = true;

                return Json(retVal);
                // return Ok(new { Success = true, Message = "Operation inserted successfully." });
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to insert operation. Error: {ErrorMessage}", e.Message);
                return BadRequest(new { Message = e.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(string itemToDelete)
        {
            Log.Information("Request to delete operation. Operation ID: {OperationId}", itemToDelete);
            if (string.IsNullOrEmpty(itemToDelete))
            {
                Log.Warning("Attempted to delete operation without specifying an operation ID.");
                return BadRequest("ItemToDelete cannot be null or empty.");
            }

            try
            {
                var deleteResult = await _operationService.DeleteAsync(int.Parse(itemToDelete));
                var changesSaved = base.UnitOfWork.SaveChangesAsync();
                Log.Information("Operation deletion status: {DeleteResult}", deleteResult);
                return Ok(new { WasDeleted = deleteResult });
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to delete operation. Error: {ErrorMessage}", e.Message);
                return BadRequest(new { Message = e.Message });
            }
        }

        public async Task<PartialViewResult> EditAsync(int id)
        {
            Log.Information("Navigated to Edit view for operation. Operation ID: {OperationId}", id);
            var retVal = new OperationVm();

            if (id <= 0)
            {
                retVal.Message = "Please provide an id.";
                return PartialView("Partial/_operationEditPartial", retVal);
            }

            var result = await _operationService.FindAsync(id);

            if (result != null)
            {
                retVal.Success = true;
                retVal.Operation = result.ConvertToViewModel();
                Log.Information("Fetched operation details for editing. {@OperationDetails}", retVal.Operation);
            }
            else
            {
                retVal.Message = "Item with requested Id was not found.";
                Log.Warning("Operation not found for editing. Operation ID: {OperationId}", id);
            }

            return PartialView("Partial/_operationEditPartial", retVal);
        }


        [AllowAnonymous]
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] Operation operation)
        {
            // https://stackoverflow.com/questions/39533599/mvc-5-with-bootstrap-modal-from-partial-view-validation-not-working
            // https://stackoverflow.com/questions/2845852/asp-net-mvc-how-to-convert-modelstate-errors-to-json
            var retVal = new OperationVm
            {
                Success = false,
                Message = string.Empty
            };

            if (ModelState.IsValid)
            {
                var valid = operation.Validate();

                try
                {
                    var existingEntity = await _operationService.FindAsync(operation.Id);

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
                    _operationService.Update(existingEntity);

                    var result = await base.UnitOfWork.SaveChangesAsync();

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
                        key = x.First().ToString().ToUpper() + string.Join(string.Empty, x.Skip(1)),
                        errors = ModelState[x].Errors.
                            Select(y => y.ErrorMessage).
                            ToArray()
                    };

                return Json(new { Success = false, FormErrors = errorModel });
            }

            return Json(new { Success = true });
        }


        [AllowAnonymous]
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            Log.Information("Accessed Detail view for operation. Operation ID: {OperationId}", id);
            var result = await _operationService.FindAsync(id);
            if (result is null)
            {
                Log.Warning("Operation details not found. Operation ID: {OperationId}", id);
                return NotFound("Item with requested Id was not found.");
            }

            var viewModel = result.ConvertToViewModel();
            Log.Information("Displaying operation details. {@ViewModel}", viewModel);
            return PartialView("Partial/_operationDetailsPartial", viewModel);
        }


        #endregion Public get

        #region CUD
        #endregion CUD

        public IActionResult Index()
        {
            return View();
        }
    }
}
