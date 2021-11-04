using PagedList;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IdentityProvider.Models.ViewModels.Operations
{
    public class OperationPagedVm
    {
        [Display(Name = "Find by")]
        public string SearchString { get; set; }
        [Display(Name = "Sort order")]
        public string SortOrder { get; set; }
        [Display(Name = "Select page size")]
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int? NumberOfDeletedItems { get; set; }
        public int? NumberOfInactiveItems { get; set; }
        public int? NumberOfActiveItems { get; set; }
        public SelectList PageSizeList { get; set; }
        public IPagedList<OperationVm> Operations { get; set; }
        public string HeaderBigText { get; set; }
        public string HeaderSmallText { get; set; }
    }

    public class OperationInsertedVm
    {
        public int WasInserted { get; set; }
        public bool Success { get; set; }
        public string ValidationIssues { get; set; }
        public string Message { get; set; }
    }

    public class OperationDeletedVm
    {
        public bool WasDeleted { get; set; }
        public string Error { get; set; }
    }

    public class OperationVm
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public OperationDto Operation { get; set; }
    }

    public class OperationDto
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int? Id { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string DeletedBy { get; set; }
        public bool UserMayViewDeletedProp { get; set; }
        public bool UserMayViewCreatedProp { get; set; }
        public bool UserMayViewLastModifieddProp { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
    }

    public class OperationToInsertVm
    {
        public OperationToInsertVm()
        {
            MakeActive = false;
        }

        [Required(ErrorMessage = "You need to set the item as either active or inactive.")]
        [StringLength(150 , MinimumLength = 1 , ErrorMessage = "The number of characters in the name may not exceed 150. ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "An operation description is required")]
        [StringLength(150 , MinimumLength = 1 , ErrorMessage = "The number of characters in the description may not exceed 150. ")]
        public string Description { get; set; }
        [DisplayName("The item will be made active (default: active). ")]
        public bool MakeActive { get; set; }
        [DisplayName("(Optional) The date when the item becomes inactive.")]
        public DateTime? ActiveUntil { get; set; }
    }
}
