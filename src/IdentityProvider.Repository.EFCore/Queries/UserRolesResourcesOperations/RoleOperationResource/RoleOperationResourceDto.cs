using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using System.Collections.Generic;

namespace IdentityProvider.Repository.EFCore.Queries.UserRolesResourcesOperations.RoleOperationResource
{
    public class RoleOperationResourceDto
    {
        public string ResourceName { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public List<Operation> Operations { get; set; }
        public string User { get; set; }
        public string Operation { get; set; }
        public string GroupName { get; set; }
        public PermissionGroupDto Group { get; set; }
    }
}