using System.Collections.Generic;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Models.ViewModels.Permissions;

namespace IdentityProvider.Repository.EF.Queries.UserRolesResourcesOperations.RoleOperationResource
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