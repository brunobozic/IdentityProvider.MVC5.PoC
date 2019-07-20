using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PagedList;

namespace IdentityProvider.Models.ViewModels.Operations
{
    public class OperationPagedVm
    {
        [Display(Name = "Find by")]
        public string SearchStringOperationsMainGrid { get; set; }
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
        public bool ShowInactive { get; set; }
        public bool ShowDeleted { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSortOrder { get; set; }

        public List<HeaderRightSideActionDropdown> HeaderRightSideActionDropdownList { get; set; }
        public string DateRangePickerOnOperationsStart { get; set; }
        public string DateRangePickerOnOperationsEnd { get; set; }
        public string OperationsDashboard_OperationsWidget_DateRange { get; set; }
        public string DateRangePickerOnOperationsStartHidden { get; set; }
        public string DateRangePickerOnOperationsEndHidden { get; set; }
    }
}
