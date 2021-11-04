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
using IdentityProvider.Models.ViewModels.Resources;
using IdentityProvider.Models.ViewModels.Resources.Extensions;
using IdentityProvider.Services.ResourceService;
using Module.Repository.EF.UnitOfWorkInterfaces;
using PagedList;
using StructureMap;

namespace IdentityProvider.Controllers.Controllers
{
    public class ResourceController : BaseController
    {
        private readonly IApplicationResourceService _resourceService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        [DefaultConstructor]
        public ResourceController(
            ICookieStorageService cookieStorageService
            , IErrorLogService errorLogService
            , IUnitOfWorkAsync unitOfWorkAsync
            , IApplicationResourceService resourceService
            , IApplicationConfiguration applicationConfiguration
        )
            : base(
                cookieStorageService
                , errorLogService
                , applicationConfiguration
            )
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _resourceService = resourceService;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ResourcesGetAllPaged(
            string sortOrder
            , string currentFilter
            , string searchString
            , int? pageNumber = 1
            , int pageSize = 10
        )
        {
            ViewBag.searchQuery = string.IsNullOrEmpty(searchString) ? string.Empty : searchString;

            pageNumber = pageNumber > 0 ? pageNumber : 1;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "Name" ? "Name_Desc" : "Name";
            ViewBag.ActiveSortParam = sortOrder == "Active" ? "Active_Desc" : "Active";
            ViewBag.DeletedSortParam = sortOrder == "Deleted" ? "Deleted_Desc" : "Deleted";
            ViewBag.DescriptionSortParam = sortOrder == "Description" ? "Description_Desc" : "Description";
            ViewBag.DateCreatedSortParam = sortOrder == "Date_Created" ? "Date_Created_Desc" : "Date_Created";
            ViewBag.DateModifiedSortParam = sortOrder == "Date_Modified" ? "Date_Modified_Desc" : "Date_Modified";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSort = sortOrder;

            // TODO: if user can see Inactive items and perhaps reactivate?
            // TODO: if user has rights to view deleted items and undelete them?
            var query = _resourceService.Queryable().Where(o => o.Active && !o.IsDeleted).Select(i =>
                new ResourceDto
                {
                    Active = i.Active,
                    Name = i.Name,
                    Description = i.Description,
                    Deleted = i.IsDeleted,
                    DateCreated = i.CreatedDate,
                    DateModified = i.ModifiedDate,
                    Id = i.Id
                });

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(s => s.Name.Contains(searchString) || s.Description.Contains(searchString));

            switch (sortOrder)
            {
                case "Name":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "Name_Desc":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "Description":
                    query = query.OrderBy(x => x.Description);
                    break;
                case "Description_Desc":
                    query = query.OrderByDescending(x => x.Description);
                    break;
                case "Active":
                    query = query.OrderBy(x => x.Active);
                    break;
                case "Active_Desc":
                    query = query.OrderByDescending(x => x.Active);
                    break;
                case "Deleted":
                    query = query.OrderBy(x => x.Deleted);
                    break;
                case "Deleted_Desc":
                    query = query.OrderByDescending(x => x.Deleted);
                    break;
                case "Date_Created":
                    query = query.OrderBy(x => x.DateCreated);
                    break;
                case "Date_Created_Desc":
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
                case "Date_Modified":
                    query = query.OrderBy(x => x.DateModified);
                    break;
                case "Date_Modified_Desc":
                    query = query.OrderByDescending(x => x.DateModified);
                    break;
                default:
                    query = query.OrderBy(x => x.Name);
                    break;
            }

            var pageNo = pageNumber ?? 1;

            var selListItem =
                CreateListOfDefaultForPaginator(out var selListItem2, out var selListItem3, out var selListItem4);

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
            var list = new SelectList(newList, "Value", "Text", null);

            var returnValue = new ResourcePagedVm
            {
                Resources = query.ToPagedList(pageNo, int.Parse(selectedOne.Text)),
                PageSize = int.Parse(selectedOne.Value),
                PageSizeList = list,
                SearchString = searchString,
                SortOrder = sortOrder
            };

            return View(returnValue);
        }

        private static SelectListItem CreateListOfDefaultForPaginator(out SelectListItem selListItem2,
            out SelectListItem selListItem3, out SelectListItem selListItem4)
        {
            // Create the select list item you want to add
            var selListItem = new SelectListItem
            {
                Text = "2",
                Value = "2",
                Selected = false
            };

            selListItem2 = new SelectListItem
            {
                Text = "10",
                Value = "10",
                Selected = false
            };

            selListItem3 = new SelectListItem
            {
                Text = "20",
                Value = "20",
                Selected = false
            };

            selListItem4 = new SelectListItem
            {
                Text = "50",
                Value = "50",
                Selected = false
            };

            return selListItem;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<JsonResult> FetchInfoOnResources()
        {
            var retVal = new InfoOnResourcesVm
            {
                ActiveItemCount = 0,
                DeletedItemCount = 0,
                InactiveItemCount = 0,
                Success = false,
                Message = string.Empty
            };

            try
            {
                var queryResult = await _unitOfWorkAsync.RepositoryAsync<ApplicationResource>().Queryable()
                    .AsNoTracking().Select(i =>
                        new ResourceCountsDto
                        {
                            Name = i.Name,
                            Active = i.Active,
                            Deleted = i.IsDeleted
                        }).ToListAsync();

                retVal.ActiveItemCount = queryResult.Count(op => op.Active && !op.Deleted);
                retVal.DeletedItemCount =
                    queryResult.Count(op => op.Deleted); // might conflict with row based access security
                retVal.InactiveItemCount = queryResult.Count(op => !op.Active);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
                _errorLogService.LogError(this, e.Message, e);
                retVal.Message = e.Message ?? string.Empty;
            }

            retVal.Success = true;

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Insert()
        {
            return PartialView("Partial/_resourceInsertPartial");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Insert(ApplicationResource resourceToInsert)
        {
            var retVal = new ResourceInsertedVm {Success = false};

            if (ModelState.IsValid)
                if (ModelState.Values.Any(i => i.Errors.Count > 0))
                {
                    var problems = ModelState.Values.Where(i => i.Errors.Count > 0).ToList();
                }

            var res = new ApplicationResource
            {
                Name = resourceToInsert.Name,
                Description = resourceToInsert.Description,
                Active = resourceToInsert.MakeActive,
                ActiveFrom = DateTime.Now,
                ActiveTo = resourceToInsert.ActiveUntil
            };

            var validationResults = res.Validate();

            if (validationResults != null && validationResults.Any())
            {
                retVal.Success = false;

                var sb = new StringBuilder();

                foreach (var validation in validationResults) sb.Append(validation.ErrorMessage);

                ModelState.AddModelError("Name", sb.ToString());
                retVal.ValidationIssues = sb.ToString();

                return Json(retVal, JsonRequestBehavior.AllowGet);
            }

            var inserted = -1;

            try
            {
                _unitOfWorkAsync.RepositoryAsync<ApplicationResource>().Insert(res);
                inserted = await _unitOfWorkAsync.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
                _errorLogService.LogError(this, e.Message, e);
                retVal.Message = e.Message ?? string.Empty;
            }

            retVal.WasInserted = inserted;
            retVal.Success = true;

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        // POST: /Resource/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Delete(string itemToDelete)
        {
            var retVal = new ResourceDeletedVm {WasDeleted = false};

            if (string.IsNullOrEmpty(itemToDelete)) throw new ArgumentNullException(nameof(itemToDelete));

            try
            {
                var repo = _unitOfWorkAsync.RepositoryAsync<ApplicationResource>();
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
                retVal.Error = e.Message ?? string.Empty;
            }

            return Json(retVal.WasDeleted, JsonRequestBehavior.AllowGet);
        }

        // GET: /Resource/Edit/5
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int? id)
        {
            var retVal = new ResourceVm
            {
                Success = false,
                Message = string.Empty
            };

            if (id <= 0)
            {
                retVal.Message = "Please provide an id.";
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return PartialView("Partial/_resourceEditPartial", retVal);
            }

            var result = _resourceService.Find(id);

            if (result != null)
            {
                retVal.Success = true;
                retVal.Resource = result.ConvertToViewModel();
            }
            else
            {
                retVal.Message = "Item with requested Id was not found.";
            }

            return PartialView("Partial/_resourceEditPartial", retVal);
        }

        // POST: /Resource/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, Name, Description, Active, ActiveFrom, ActiveTo")]
            ApplicationResource resource)
        {
            // https://stackoverflow.com/questions/39533599/mvc-5-with-bootstrap-modal-from-partial-view-validation-not-working
            // https://stackoverflow.com/questions/2845852/asp-net-mvc-how-to-convert-modelstate-errors-to-json
            var retVal = new ResourceVm
            {
                Success = false,
                Message = string.Empty
            };

            if (ModelState.IsValid)
            {
                var repo = _unitOfWorkAsync.RepositoryAsync<ApplicationResource>();

                var valid = resource.Validate();

                try
                {
                    repo.Update(resource);

                    var result = await _unitOfWorkAsync.SaveChangesAsync();

                    if (result > 0) retVal.Success = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Debug.WriteLine(e);
                    retVal.Message = e.Message;
                }
            }
            else
            {
                retVal.FormErrors = ModelState.Select(kvp => new
                    {key = kvp.Key, errors = kvp.Value.Errors.Select(e => e.ErrorMessage)});
                retVal.Message = "Model state invalid";
            }

            return PartialView("Partial/_resourceEditPartial", retVal);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<ActionResult> Detail(int id)
        {
            var retVal = new ResourceVm
            {
                Success = false,
                Message = string.Empty
            };

            if (id <= 0)
            {
                retVal.Message = "Please provide an id.";
                return PartialView("Partial/_resourceDetailsPartial", retVal);
            }

            var result = await _resourceService.FindAsync(id);

            if (result != null)
            {
                retVal.Success = true;

                if (Request.IsAjaxRequest()) retVal.Resource = result.ConvertToViewModel();
            }
            else
            {
                retVal.Message = "Item with requested Id was not found.";
            }

            return PartialView("Partial/_resourceDetailsPartial", retVal);
        }
    }
}