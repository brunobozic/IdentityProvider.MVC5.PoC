using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace Module.CrossCutting.Models.ViewModels.Operations
{
    public class OperationAuditTrailPagedVm
    {
        [Display(Name = "Find by")] public string SearchString { get; set; }

        [Display(Name = "Sort order")] public string SortOrder { get; set; }

        [Display(Name = "Select page size")] public int PageSize { get; set; }

        public int PageCount { get; set; }
        public int PageNumber { get; set; }

        public SelectList PageSizeList { get; set; }
        public IPagedList<OperationAuditTrailVm> OperationAuditTrail { get; set; }
        public string HeaderBigText { get; set; }
        public string HeaderSmallText { get; set; }
    }
}