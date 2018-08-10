
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Repository.EF.Queries.UserRolesResourcesOperations;
using IdentityProvider.Services.OperationsService;
using Module.Repository.EF.UnitOfWorkInterfaces;
using PagedList;

namespace IdentityProvider.Controllers.Controllers
{

    public class OperationController : BaseController, IController
    {

        private readonly IOperationService _operationService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

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

        public ActionResult OperationsGetAllPaged(
            string sortOrder
            , string currentFilter
            , string searchString
            , int pageNumber = 1
            , int pageSize = 25
            )
        {

            ViewBag.searchQuery = string.IsNullOrEmpty(searchString) ? "" : searchString;

            pageNumber = pageNumber > 0 ? pageNumber : 1;
            pageSize = pageSize > 0 ? pageSize : 25;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "Name" ? "Name_Desc" : "Name";
            ViewBag.ActiveSortParam = sortOrder == "Active" ? "Active_Desc" : "Active";
            ViewBag.ActiveSortParam = sortOrder == "Deleted" ? "Deleted_Desc" : "Deleted";
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
                new OperationPagedVm
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
                case "Description":
                    query = query.OrderBy(x => x.Description);
                    break;
                case "Active":
                    query = query.OrderByDescending(x => x.Active);
                    break;
                case "Deleted":
                    query = query.OrderBy(x => x.Deleted);
                    break;
                case "Date_Created":
                    query = query.OrderBy(x => x.DateCreated);
                    break;
                case "Date_Created_Desc":
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
                default:
                    break;
            }

            return View(query.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult OperationInsert(OperationDto operationToInsert)
        {
            OperationInsertedVm retVal = new OperationInsertedVm();
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

        public async Task<ActionResult> OperationDelete(int operationToDelete)
        {
            var retVal = new OperationDeletedVm();

            var returnValue = await _operationService.DeleteAsync(operationToDelete);
            var deleted = _unitOfWorkAsync.SaveChanges();

            retVal.WasDeleted = deleted;

            return View(retVal);
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
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int Id { get; set; }
    }

    public class OperationInsertedVm
    {
        public int WasInserted { get; set; }
    }

    public class OperationDeletedVm
    {
        public int WasDeleted { get; set; }
    }

    public class OperationVm
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
        public int Id { get; set; }
    }
}
