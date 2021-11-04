using System.Collections.Generic;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Repository.EF.EFDataContext;
using Module.Repository.EF;
using Module.Repository.EF.UnitOfWorkInterfaces;

namespace IdentityProvider.Repository.EF.Queries.UserRolesResourcesOperations.UserRoleResourcesOperations
{
    public class UserRoleResourcesOperationsQuery: QueryObject<ApplicationRole>
    { 
        public bool OnlyActiveRoles { get; set; }
        public bool OnlyActiveOperations { get; set; }
        public bool OnlyActiveResources { get; set; }
        public string RoleNameLike { get; set; }
        public string OperationNameLike { get; set; }
        public string ResourceNameLike { get; set; }
    }
}