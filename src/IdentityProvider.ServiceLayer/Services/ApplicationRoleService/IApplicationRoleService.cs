using IdentityProvider.Repository.EFCore.Queries.UserRolesResourcesOperations.RoleOperationResource;
using IdentityProvider.Repository.EFCore.Queries.UserRolesResourcesOperations.UserRoleResourcesOperations;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityProvider.ServiceLayer.Services.RoleService
{
    public interface IRoleService
    {
        Task<IdentityResult> AddRoleAsync(string roleName, string optionalDescription, bool startAsNonActive = false);

        IEnumerable<RoleOperationResourceDto> FetchUserRoleResourceAndOperationsGraph();

        Task<IEnumerable<UserRoleResourcesOperationsDto>> FetchResourceAndOperationsGraphAsync();
    }
}