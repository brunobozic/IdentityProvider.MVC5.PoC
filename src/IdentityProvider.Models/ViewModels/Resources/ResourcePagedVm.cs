using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PagedList;

namespace IdentityProvider.Models.ViewModels.Resources
{
    public class ResourcePagedVm
    {
        [Display(Name = "Find by")]
        public string SearchString { get; set; }
        [Display(Name = "Sort order")]
        public string SortOrder { get; set; }
        [Display(Name = "Select page size")]
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public SelectList PageSizeList { get; set; }
        public IPagedList<ResourceVm> Resources { get; set; }
    }

    public class ResourceVm
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
        public bool UserMayViewDeletedResource { get; set; }
        public bool UserMayViewCreatedResource { get; set; }
        public bool UserMayViewLastModifieddREsource { get; set; }
    }
}