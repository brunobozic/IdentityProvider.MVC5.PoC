using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using System.Collections.Generic;

namespace IdentityProvider.Repository.EFCore.Queries.UserRolesResourcesOperations.UserRoleResourcesOperations
{
    public class UserRoleResourcesOperationsDto
    {
        public string ResourceName { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public List<Operation> Operations { get; set; }
        public string User { get; set; }
        public string Operation { get; set; }
    }
}