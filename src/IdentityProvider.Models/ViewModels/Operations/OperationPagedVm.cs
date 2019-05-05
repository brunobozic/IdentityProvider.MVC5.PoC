using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PagedList;

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
        public bool ShowInactive { get; set; }
    }
}
