using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using System.Collections.Generic;

namespace IdentityProvider.Repository.EFCore.Queries.UserRolesResourcesOperations
{
    public class ResourceOperationDto
    {
        public Operation Operation { get; set; }
        public string ResourceName { get; set; }
        public string ResourceDescription { get; set; }
        public bool ResourceActive { get; set; }
        public ICollection<Operation> Operations { get; set; }
        public bool ResourceDeleted { get; set; }
    }
}