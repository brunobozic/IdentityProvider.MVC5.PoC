
using AutoMapper;

using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using IdentityProvider.ServiceLayer.Services.OperationsService;
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
using System.Linq;
using System.Threading.Tasks;
using URF.Core.Abstractions;


namespace IdentityProvider.Web.MVC6.Controllers
{

    [Route("api/operation")]
    [ApiController]
    [Produces("application/json")]
    public class OperationsController : BaseController, IController
    {
        #region Private props

        private readonly IOperationService _operationService;
        private readonly ILogger<OperationsController> _logger;

        #endregion Private props

        #region Ctor

        public OperationsController(
            IUnitOfWork unitOfWork
            , IMapper mapper
            , IOptionsSnapshot<ApplicationSettings> configurationValues
            , IOperationService operationService
            , IMemoryCache memCache
            , IHttpContextAccessor contextAccessor
            , IConfiguration configuration
            , ICookieStorageService cookieStorageService
            , ILogger<OperationsController> logger
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
        #endregion Public Paginated get

        #region Public get

        [HttpGet("OperationsGetAllPaged")]
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

            return Json(new
            {
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = data
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

                return Ok(retVal);
            }
            catch (Exception e)
            {
                Log.Error(e, "An error occurred while fetching information on operations.");
                return BadRequest(new { Message = e.Message });
            }
        }


        [HttpGet("Insert")]
        public IActionResult Insert()
        {
            Log.Information("Navigated to Insert view for creating a new operation.");
            return PartialView("Partial/_operationInsertPartial");
        }


        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] OperationToInsertVm operationToInsertVm)
        {
            Log.Information("Attempting to insert a new operation: {@OperationToInsertVm}", operationToInsertVm);
            if (!ModelState.IsValid)
            {
                Log.Warning("Model state is invalid. Errors: {@ModelStateErrors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);
            }

            try
            {
                var operation = Mapper.Map<Operation>(operationToInsertVm);
                _operationService.Insert(operation);
                await UnitOfWork.SaveChangesAsync();
                Log.Information("Operation inserted successfully. Operation ID: {OperationId}", operation.Id);

                return Ok(new { Success = true, Message = "Operation inserted successfully." });
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to insert operation. Error: {ErrorMessage}", e.Message);
                return BadRequest(new { Message = e.Message });
            }
        }


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



        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] Operation operation)
        {
            Log.Information("Attempting to edit operation: {@Operation}", operation);
            if (!ModelState.IsValid)
            {
                Log.Warning("Model state is invalid while editing operation. Errors: {@ModelStateErrors}", ModelState.Values.SelectMany(v => v.Errors));
                return Json(new { Success = false, FormErrors = ModelState });
            }

            try
            {
                // Your existing code for updating the operation
                Log.Information("Operation edited successfully. Operation ID: {OperationId}", operation.Id);
                return Json(new { Success = true });
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to edit operation. Operation ID: {OperationId}, Error: {ErrorMessage}", operation.Id, e.Message);
                return BadRequest(new { Message = e.Message });
            }
        }



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
