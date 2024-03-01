
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions;


namespace IdentityProvider.Web.MVC6.Controllers
{

    [Route("api/operation")]
    [ApiController]
    [Produces("application/json")]
    public class OperationsController : BaseController, IController
    {
        private readonly IOperationService _operationService;

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
                Actions = string.Format("<a href=\"\" class=\"OperationsDashboard_OperationsDatatable_edit text-center\" data-id=\"{0}\" custom-middle-align\">Edit</a> / <a href=\"\" class=\"OperationsDashboard_OperationsDatatable_remove\" data-id=\"{0}\">Delete</a>", s.Id)
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
            var alsoinactive = model.alsoinactive;
            var alsodeleted = model.alsodeleted;
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
                , alsoinactive
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



        [HttpGet("FetchInfoOnOperations")]
        public async Task<IActionResult> FetchInfoOnOperations()
        {
            try
            {
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

                return Ok(retVal);
            }
            catch (Exception e)
            {
                // Log error here
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpGet("Insert")]
        public IActionResult Insert()
        {
            return PartialView("Partial/_operationInsertPartial");
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] OperationToInsertVm operationToInsertVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                // This maps the view model to your domain entity.
                var operation = base.Mapper.Map<Operation>(operationToInsertVm);

                // If you're not using AutoMapper, you could manually map the properties like this:
                // var operation = new Operation
                // {
                //     Name = operationToInsertVm.Name,
                //     Description = operationToInsertVm.Description,
                //     Active = operationToInsertVm.MakeActive,
                //     ActiveTo = operationToInsertVm.ActiveUntil,
                //     ActiveFrom = operationToInsertVm.MakeActive ? DateTime.UtcNow : (DateTime?)null
                // };

                _operationService.Insert(operation);
                await base.UnitOfWork.SaveChangesAsync();


                return Ok(new { Success = true, Message = "Operation inserted successfully." });
            }
            catch (Exception e)
            {
                // Log the exception here
                return BadRequest(new { Message = e.Message });
            }
        }


        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(string itemToDelete)
        {
            if (string.IsNullOrEmpty(itemToDelete))
            {
                return BadRequest("ItemToDelete cannot be null or empty.");
            }

            try
            {
                var deleteResult = await _operationService.DeleteAsync(int.Parse(itemToDelete));
                return Ok(new { WasDeleted = deleteResult });
            }
            catch (Exception e)
            {
                // Log error here
                return BadRequest(new { Message = e.Message });
            }
        }


        public async Task<PartialViewResult> EditAsync(int id)
        {
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
            }
            else
            {
                retVal.Message = "Item with requested Id was not found.";
            }

            return PartialView("Partial/_operationEditPartial", retVal);

        }


        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] Operation operation)
        {

            // https://stackoverflow.com/questions/39533599/mvc-5-with-bootstrap-modal-from-partial-view-validation-not-working
            // https://stackoverflow.com/questions/2845852/asp-net-mvc-how-to-convert-modelstate-errors-to-json
            var retVal = new OperationVm();

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


        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Please provide a valid id.");
            }

            var result = await _operationService.FindAsync(id);
            if (result is null)
            {
                return NotFound("Item with requested Id was not found.");
            }

            // Assuming ConvertToViewModel is an extension method or similar.
            var viewModel = result.ConvertToViewModel();
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
