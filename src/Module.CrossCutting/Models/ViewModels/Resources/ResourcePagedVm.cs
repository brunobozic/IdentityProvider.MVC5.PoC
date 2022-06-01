using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace Module.CrossCutting.Models.ViewModels.Resources
{
    public class ResourcePagedVm
    {
        [Display(Name = "Find by")] public string SearchString { get; set; }

        [Display(Name = "Sort order")] public string SortOrder { get; set; }

        [Display(Name = "Select page size")] public int PageSize { get; set; }

        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int? NumberOfDeletedItems { get; set; }
        public int? NumberOfInactiveItems { get; set; }
        public int? NumberOfActiveItems { get; set; }
        public SelectList PageSizeList { get; set; }
        public IPagedList<ResourceDto> Resources { get; set; }
        public string HeaderBigText { get; set; }
        public string HeaderSmallText { get; set; }
    }
}