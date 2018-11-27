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
using IdentityProvider.Services.AuditTrailService;
using IdentityProvider.Services.OperationsService;
using Module.Repository.EF.UnitOfWorkInterfaces;
using PagedList;
using StructureMap;
using TrackableEntities;

namespace IdentityProvider.Controllers.Controllers
{
    public class OperationController : BaseController
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
            , string from 
            , string to
            , int? pageNumber = 1
            , int pageSize = 10
            , bool ShowInactive = false
        )
        {
            searchString = SetupViewbagForPagedItems(sortOrder , currentFilter , searchString , ref pageNumber);

            // TODO: if user can see Inactive items and perhaps reactivate?
            // TODO: if user has rights to view deleted items and undelete them?

            IQueryable<OperationVm> query;

            if (ShowInactive)
            {
                query = _operationService.Queryable().Where(o => !o.IsDeleted).Select(i =>
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
            }
            else
            {
                query = _operationService.Queryable().Where(o => o.Active && !o.IsDeleted).Select(i =>
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
                           Id = i.Id ,
                       }
                   });
            }

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

            var pageNo = SetupListOfPageSizes(pageNumber , pageSize , out var selectedOne , out var list);

            var returnValue = new OperationPagedVm
            {
                Operations = query.ToPagedList(pageNo , int.Parse(selectedOne.Text)) ,
                PageSize = int.Parse(selectedOne.Value) ,
                PageSizeList = list ,
                SearchString = searchString ,
                SortOrder = sortOrder ,
                ShowInactive = ShowInactive
            };

            return View(returnValue);
        }

        private static int SetupListOfPageSizes( int? pageNumber , int pageSize , out SelectListItem selectedOne ,
            out SelectList list )
        {
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
            selectedOne = newList.Single(i => i.Text == pageSize.ToString());
            selectedOne.Selected = true;

            // Return the list of selectlistitems as a selectlist
            list = new SelectList(newList , "Value" , "Text" , null);

            return pageNo;
        }

        private string SetupViewbagForPagedItems( string sortOrder , string currentFilter , string searchString ,
            ref int? pageNumber )
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

            return searchString;
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
        public ActionResult Insert()
        {
            return PartialView("Partial/_operationInsertPartial");
        }

        // POST: /Operation/Insert/5
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> Insert( OperationToInsertVm operationToInsert )
        {
            var retVal = new OperationInsertedVm { Success = false };

            if (!ModelState.IsValid)
            {
                if (ModelState.Values.Any(i => i.Errors.Count > 0))
                {
                    var problems = ModelState.Values.Where(i => i.Errors.Count > 0).ToList();
                }

                retVal.Message = "Model state invalid";
                retVal.FormErrors = ModelState.Select(kvp => new { key = kvp.Key , errors = kvp.Value.Errors.Select(e => e.ErrorMessage) });
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

        // POST: /Operation/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> Delete( string itemToDelete )
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
                _errorLogService.LogError(this , e.Message , e);
                retVal.Error = e.Message ?? "";
            }

            return Json(retVal.WasDeleted , JsonRequestBehavior.AllowGet);
        }

        // GET: /Operation/Edit/5
        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult Edit( int id )
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

        // POST: /Operation/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( [Bind(Include = "Id, Name, Description, Active, ActiveFrom, ActiveTo, RowVersion")] Operation operation )
        {
            // https://stackoverflow.com/questions/39533599/mvc-5-with-bootstrap-modal-from-partial-view-validation-not-working
            // https://stackoverflow.com/questions/2845852/asp-net-mvc-how-to-convert-modelstate-errors-to-json
            var retVal = new OperationVm
            {
                Success = false ,
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
                        ModelState.AddModelError(string.Empty , @"Unable to update entity. The entity was deleted by another user.");
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
                        return Json(new { Success = false , OptimisticConcurrencyError = true , OptimisticConcurrencyErrorMsg = "The record you attempted to edit was modified by another user after you got the original value. The edit operation was canceled and the current values in the database have been displayed." });
                    }
                }
            }
            else
            {
                // the model aint valid, we need to return the user to the view to enable him to fix the entry...
                retVal.Success = false;

                var errorModel =
                    from x in ModelState.Keys
                    where ModelState[ x ].Errors.Count > 0
                    select new
                    {
                        key = x.First().ToString().ToUpper() + string.Join("" , x.Skip(1)) ,
                        errors = ModelState[ x ].Errors.
                            Select(y => y.ErrorMessage).
                            ToArray()
                    };

                return Json(new { Success = false , FormErrors = errorModel });
            }

            return Json(new { Success = true });
        }

        // POST: /Operation/Detail/5
        // GET: /Operation/Detail/5
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<PartialViewResult> Detail( int id )
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

     

        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //public ActionResult OperationAuditTrailGetAllPaged(
        //    string sortOrder
        //    , string currentFilter
        //    , string searchString
        //    , int? pageNumber = 1
        //    , int pageSize = 10
        //    )
        //{
        //    searchString = SetupViewbagForPagedItems(sortOrder , currentFilter , searchString , ref pageNumber);

        //    var query = _auditTrailService.Queryable().Select(i =>
        //        new OperationAuditTrailVm
        //        {
        //            OperationAuditTrail = new OperationAuditTrailDto
        //            {
        //                Id = i.Id ,
        //                TableName = i.TableName ,
        //                UserName = i.UserName

        //            }
        //        });

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        query = query.Where(s =>
        //           s.OperationAuditTrail.TableName.Contains(searchString)
        //        || s.OperationAuditTrail.OldData.Contains(searchString)
        //        || s.OperationAuditTrail.NewData.Contains(searchString));
        //    }

        //    //switch (sortOrder)
        //    //{
        //    //    case "Name":
        //    //        query = query.OrderBy(x => x.Name);
        //    //        break;
        //    //    case "Name_Desc":
        //    //        query = query.OrderByDescending(x => x.Name);
        //    //        break;
        //    //    case "Description":
        //    //        query = query.OrderBy(x => x.Description);
        //    //        break;
        //    //    case "Description_Desc":
        //    //        query = query.OrderByDescending(x => x.Description);
        //    //        break;
        //    //    case "Active":
        //    //        query = query.OrderBy(x => x.Active);
        //    //        break;
        //    //    case "Active_Desc":
        //    //        query = query.OrderByDescending(x => x.Active);
        //    //        break;
        //    //    case "Deleted":
        //    //        query = query.OrderBy(x => x.Deleted);
        //    //        break;
        //    //    case "Deleted_Desc":
        //    //        query = query.OrderByDescending(x => x.Deleted);
        //    //        break;
        //    //    case "Date_Created":
        //    //        query = query.OrderBy(x => x.DateCreated);
        //    //        break;
        //    //    case "Date_Created_Desc":
        //    //        query = query.OrderByDescending(x => x.DateCreated);
        //    //        break;
        //    //    case "Date_Modified":
        //    //        query = query.OrderBy(x => x.DateModified);
        //    //        break;
        //    //    case "Date_Modified_Desc":
        //    //        query = query.OrderByDescending(x => x.DateModified);
        //    //        break;
        //    //    default:
        //    //        query = query.OrderBy(x => x.Name);
        //    //        break;
        //    //}

        //    var pageNo = SetupListOfPageSizes(pageNumber , pageSize , out var selectedOne , out var list);

        //    var returnValue = new OperationAuditTrailPagedVm
        //    {
        //        OperationAuditTrail = query.ToPagedList(pageNo , int.Parse(selectedOne.Text)) ,
        //        PageSize = int.Parse(selectedOne.Value) ,
        //        PageSizeList = list ,
        //        SearchString = searchString ,
        //        SortOrder = sortOrder ,
        //    };

        //    return Json(returnValue);
        //}
    }
}

