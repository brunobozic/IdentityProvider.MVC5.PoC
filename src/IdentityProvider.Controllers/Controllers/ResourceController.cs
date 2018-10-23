using System;
using System.Collections.Generic;
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
        public ActionResult ResourceGetAllPaged(
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

            var query = _resourceService.Queryable().Where(o => o.Active && !o.IsDeleted).Select(i =>
                new ResourceVm
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
            {
                query = query.Where(s => s.Name.Contains(searchString) || s.Description.Contains(searchString));
            }

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

            var pageNo = (pageNumber ?? 1);

            // Create the select list item you want to add
            var selListItem = new SelectListItem
            {
                Text = "2",
                Value = "2",
                Selected = false
            };

            var selListItem2 = new SelectListItem
            {
                Text = "10",
                Value = "10",
                Selected = false
            };

            var selListItem3 = new SelectListItem
            {
                Text = "20",
                Value = "20",
                Selected = false
            };

            var selListItem4 = new SelectListItem
            {
                Text = "50",
                Value = "50",
                Selected = false
            };

            // Create a list of select list items - this will be returned as your select list
            var newList = new List<SelectListItem> { selListItem, selListItem2, selListItem3, selListItem4 };    //Add select list item to list of selectlistitems

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ResourceInsert(ApplicationResource resourceToInsert)
        {
            var retVal = new ResourceInsertedVm();

            var validationResults = resourceToInsert.Validate();

            if (validationResults != null && validationResults.Any())
            {
                retVal.Success = false;

                var sb = new StringBuilder();

                foreach (var validation in validationResults)
                {
                    sb.Append(validation.ErrorMessage);
                }

                return View("Partial/_failedToInsertResourcePartial", retVal);
            }

            _resourceService.Insert(resourceToInsert);

            var inserted = _unitOfWorkAsync.SaveChanges();

            retVal.WasInserted = inserted;
            retVal.Success = true;

            return View(retVal);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<ActionResult> ResourceDeleteAsync(string resourceToDelete)
        {
            var retVal = new OperationDeletedVm { WasDeleted = false };

            if (string.IsNullOrEmpty(resourceToDelete))
            {
                throw new ArgumentNullException(nameof(resourceToDelete));
            }

            try
            {
                var repo = _unitOfWorkAsync.RepositoryAsync<ApplicationResource>();
                await repo.DeleteAsync(int.Parse(resourceToDelete));
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

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<ActionResult> ResourceEditAsync(object id)
        {
            var result = await _resourceService.FindAsync(id);

            switch (result)
            {
                case null:
                    return PartialView("Partial/_resourceNotFoundPartial");
                default:
                    var viewModel = result.ConvertToViewModel();
                    return PartialView("Partial/_resourceEditPartial", viewModel);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<ActionResult> ResourceDetailsAsync(object id)
        {
            var result = await _resourceService.FindAsync(id);

            switch (result)
            {
                case null:
                    return PartialView("Partial/_resourceNotFoundPartial");
                default:
                    var viewModel = result.ConvertToViewModel();
                    return PartialView("Partial/_resourceDetailsPartial", viewModel);
            }
        }
    }
}
