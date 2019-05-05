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
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Models.ViewModels.Operations;
using IdentityProvider.Models.ViewModels.Operations.Extensions;
using IdentityProvider.Services.OperationsService;
using Module.Repository.EF.UnitOfWorkInterfaces;
using PagedList;
using StructureMap;

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
            string sortOrder
            , string currentFilter
            , string searchString
            , int? pageNumber = 1
            , int pageSize = 10
        )
        {
            ViewBag.searchQuery = string.IsNullOrEmpty(searchString) ? "" : searchString;

            pageNumber = pageNumber > 0 ? pageNumber : 1;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "Name" ? "Name_Desc" : "Name";
            ViewBag.ActiveSortParam = sortOrder == "Active" ? "Active_Desc" : "Active";
            ViewBag.DeletedSortParam = sortOrder == "Deleted" ? "Deleted_Desc" : "Deleted";
            ViewBag.DescriptionSortParam = sortOrder == "Description" ? "Description_Desc" : "Description";
            ViewBag.DateCreatedSortParam = sortOrder == "Date_Created" ? "Date_Created_Desc" : "Date_Created";
            ViewBag.DateModifiedSortParam = sortOrder == "Date_Modified" ? "Date_Modified_Desc" : "Date_Modified";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSort = sortOrder;

            // TODO: if user can see Inactive items and perhaps reactivate?
            // TODO: if user has rights to view deleted items and undelete them?
            var query = _operationService.Queryable().Where(o => o.Active && !o.IsDeleted).Select(i =>
                new OperationVm
                {
                    Operation = new OperationDto
                    {
                        Active = i.Active ,
                        Name = i.Name ,
                        Description = i.Description ,
                        Deleted = i.IsDeleted ,
                        DateCreated = i.CreatedDate ,
                        DateModified = i.ModifiedDate ,
                        Id = i.Id
                    }
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Operation.Name.Contains(searchString) || s.Operation.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    query = query.OrderBy(x => x.Operation.Name);
                    break;
                case "Name_Desc":
                    query = query.OrderByDescending(x => x.Operation.Name);
                    break;
                case "Description":
                    query = query.OrderBy(x => x.Operation.Description);
                    break;
                case "Description_Desc":
                    query = query.OrderByDescending(x => x.Operation.Description);
                    break;
                case "Active":
                    query = query.OrderBy(x => x.Operation.Active);
                    break;
                case "Active_Desc":
                    query = query.OrderByDescending(x => x.Operation.Active);
                    break;
                case "Deleted":
                    query = query.OrderBy(x => x.Operation.Deleted);
                    break;
                case "Deleted_Desc":
                    query = query.OrderByDescending(x => x.Operation.Deleted);
                    break;
                case "Date_Created":
                    query = query.OrderBy(x => x.Operation.DateCreated);
                    break;
                case "Date_Created_Desc":
                    query = query.OrderByDescending(x => x.Operation.DateCreated);
                    break;
                case "Date_Modified":
                    query = query.OrderBy(x => x.Operation.DateModified);
                    break;
                case "Date_Modified_Desc":
                    query = query.OrderByDescending(x => x.Operation.DateModified);
                    break;
                default:
                    query = query.OrderBy(x => x.Operation.Name);
                    break;
            }

            var pageNo = ( pageNumber ?? 1 );

            var selListItem = CreateListOfDefaultForPaginator(out var selListItem2 , out var selListItem3 , out var selListItem4);

            // Create a list of select list items - this will be returned as your select list
            var newList =
                new List<SelectListItem>
                {
                    selListItem,
                    selListItem2,
                    selListItem3,
                    selListItem4
                }; //Add select list item to list of selectlistitems

            // Based on the incoming parameter, set one of the list items to selected equals true
            var selectedOne = newList.Single(i => i.Text == pageSize.ToString());
            selectedOne.Selected = true;

            // Return the list of selectlistitems as a selectlist
            var list = new SelectList(newList , "Value" , "Text" , null);

            var returnValue = new OperationPagedVm
            {
                Operations = query.ToPagedList(pageNo , int.Parse(selectedOne.Text)) ,
                PageSize = int.Parse(selectedOne.Value) ,
                PageSizeList = list ,
                SearchString = searchString ,
                SortOrder = sortOrder
            };

            return View(returnValue);
        }

        private static SelectListItem CreateListOfDefaultForPaginator( out SelectListItem selListItem2 ,
            out SelectListItem selListItem3 , out SelectListItem selListItem4 )
        {
            // Create the select list item you want to add
            var selListItem = new SelectListItem
            {
                Text = "2" ,
                Value = "2" ,
                Selected = false
            };

            selListItem2 = new SelectListItem
            {

                Text = "10" ,
                Value = "10" ,
                Selected = false
            };

            selListItem3 = new SelectListItem
            {
                Text = "20" ,
                Value = "20" ,
                Selected = false
            };

            selListItem4 = new SelectListItem
            {
                Text = "50" ,
                Value = "50" ,
                Selected = false
            };

            return selListItem;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<JsonResult> FetchInfoOnOperations()
        {
            var retVal = new InfoOnOperationsVm
            {
                ActiveItemCount = 0 ,
                DeletedItemCount = 0 ,
                InactiveItemCount = 0 ,
                Success = false ,
                Message = string.Empty
            };

            try
            {
                var queryResult = await _unitOfWorkAsync.RepositoryAsync<Operation>().Queryable().AsNoTracking().Select(i =>
                    new OperationCountsDto
                    {
                        Name = i.Name ,
                        Active = i.Active ,
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
                _errorLogService.LogError(this , e.Message , e);
                retVal.Message = e.Message ?? "";
            }

            retVal.Success = true;

            return Json(retVal , JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult OperationInsert()
        {
            return PartialView("Partial/_operationInsertPartial");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> OperationInsert( OperationToInsertVm operationToInsert )
        {
            var retVal = new OperationInsertedVm { Success = false };

            if (ModelState.IsValid)
            {
                if (ModelState.Values.Any(i => i.Errors.Count > 0))
                {
                    var problems = ModelState.Values.Where(i => i.Errors.Count > 0).ToList();
                }
            }

            var op = new Operation
            {
                Name = operationToInsert.Name ,
                Description = operationToInsert.Description ,
                Active = operationToInsert.MakeActive ,
                ActiveFrom = DateTime.Now ,
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

                ModelState.AddModelError("Name" , sb.ToString());
                retVal.ValidationIssues = sb.ToString();

                return Json(retVal , JsonRequestBehavior.AllowGet);
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
                _errorLogService.LogError(this , e.Message , e);
                retVal.Message = e.Message ?? "";
            }

            retVal.WasInserted = inserted;
            retVal.Success = true;

            return Json(retVal , JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<JsonResult> OperationDeleteAsync( string operationToDelete )
        {
            var retVal = new OperationDeletedVm { WasDeleted = false };

            if (string.IsNullOrEmpty(operationToDelete))
            {
                throw new ArgumentNullException(nameof(operationToDelete));
            }

            try
            {
                var repo = _unitOfWorkAsync.RepositoryAsync<Operation>();
                await repo.DeleteAsync(int.Parse(operationToDelete));
                var result = await _unitOfWorkAsync.SaveChangesAsync();

                if (result > 0)
                    retVal.WasDeleted = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
                _errorLogService.LogError(this , e.Message , e);
                retVal.Error = e.Message ?? "";
            }

            return Json(retVal.WasDeleted , JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult OperationEdit( int id )
        {
            var retVal = new OperationVm
            {
                Success = false ,
                Message = ""
            };

            if (id <= 0)
            {
                retVal.Message = "Please provide an id.";
                return PartialView("Partial/_operationEditPartial" , retVal);
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

            return PartialView("Partial/_operationEditPartial" , retVal);

        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<PartialViewResult> OperationDetails( int id )
        {
            var retVal = new OperationVm
            {
                Success = false ,
                Message = ""
            };

            if (id <= 0)
            {
                retVal.Message = "Please provide an id.";
                return PartialView("Partial/_operationDetailsPartial" , retVal);
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

            return PartialView("Partial/_operationDetailsPartial" , retVal);
        }
    }

    public class OperationCountsDto
    {
        public int ActiveItemCount { get; set; }
        public int DeletedItemCount { get; set; }
        public int InactiveItemCount { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
    }

    public class InfoOnOperationsVm
    {
        public int ActiveItemCount { get; set; }
        public int DeletedItemCount { get; set; }
        public int InactiveItemCount { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

