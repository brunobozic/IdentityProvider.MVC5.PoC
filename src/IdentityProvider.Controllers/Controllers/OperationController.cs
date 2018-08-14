
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Repository.EF.Queries.UserRolesResourcesOperations;
using IdentityProvider.Services.OperationsService;
using Module.Repository.EF.UnitOfWorkInterfaces;
using PagedList;
using StructureMap;

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
            )
            : base(cookieStorageService, errorLogService)
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

            var query = _operationService.Queryable().Where(o => o.Active && !o.IsDeleted).Select(i =>
                new OperationVm
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

            //Create the select list item you want to add
            var selListItem = new SelectListItem
            {
                Text = "2",
                Value = "1",
                Selected = false
            };

            var selListItem2 = new SelectListItem
            {
                Text = "10",
                Value = "2",
                Selected = false
            };

            var selListItem3 = new SelectListItem
            {
                Text = "20",
                Value = "3",
                Selected = false
            };

            var selListItem4 = new SelectListItem
            {
                Text = "50",
                Value = "4",
                Selected = false
            };

            //Create a list of select list items - this will be returned as your select list
            var newList = new List<SelectListItem> { selListItem, selListItem2, selListItem3, selListItem4 };    //Add select list item to list of selectlistitems

            // Based on the incoming parameter, set one of the list items to selected equals true
            var selectedOne = newList.Single(i => i.Text == pageSize.ToString());
            selectedOne.Selected = true;

            //Return the list of selectlistitems as a selectlist
            var list = new SelectList(newList, "Value", "Text", null);

            var returnValue = new OperationPagedVm
            {
                Operations = query.ToPagedList(pageNo, Int32.Parse(selectedOne.Text)),
                PageSize = Int32.Parse(selectedOne.Text),
                PageSizeList = list,
                SearchString = searchString,
                SortOrder = sortOrder
            };

            return View(returnValue);
        }

        public ActionResult OperationInsert(OperationDto operationToInsert)
        {
            var retVal = new OperationInsertedVm();

            var op = new Operation
            {
                Active = true,
                IsDeleted = false,
                Description = operationToInsert.Description,
                Name = operationToInsert.Name
            };

            _operationService.Insert(op);

            var inserted = _unitOfWorkAsync.SaveChanges();
            retVal.WasInserted = inserted;

            return View(retVal);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult OperationDelete(string operationToDelete)
        {
            var retVal = new OperationDeletedVm { WasDeleted = false };
            if (!string.IsNullOrEmpty(operationToDelete))
            {
                // _operationService.Delete(int.Parse(operationToDelete));

                try
                {
                    var repo = _unitOfWorkAsync.Repository<Operation>();
                    repo.Delete(int.Parse(operationToDelete));
                    var result = _unitOfWorkAsync.SaveChanges();

                    if (result > 0)
                        retVal.WasDeleted = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Debug.WriteLine(e);

                    throw;
                }

            }
            else
            {
                retVal.WasDeleted = false;
            }

            return Json(retVal.WasDeleted, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OperationEdit(object id)
        {
            throw new NotImplementedException();
        }

        public ActionResult OperationDetails(object id)
        {
            throw new NotImplementedException();
        }

    }

    public class OperationPagedVm
    {
        [Display(Name = "Find by")]
        public string SearchString { get; set; }
        public string SortOrder { get; set; }
        [Display(Name = "Select page size")]
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public SelectList PageSizeList { get; set; }
        public IPagedList<OperationVm> Operations { get; set; }
    }

    public class OperationInsertedVm
    {
        public int WasInserted { get; set; }
    }

    public class OperationDeletedVm
    {
        public bool WasDeleted { get; set; }
    }

    public class OperationVm
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int Id { get; set; }
    }
}
