using System;

namespace IdentityProvider.Models.ViewModels.Resources
{
    public class ResourceDto
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int? Id { get; set; }
        public string CreatedBy { get; set; }
        public string DeletedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateDeleted { get; set; }
        public bool UserMayViewCreatedProp { get; set; }
        public bool UserMayViewDeletedProp { get; set; }
        public bool UserMayViewLastModifieddProp { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
    }
}