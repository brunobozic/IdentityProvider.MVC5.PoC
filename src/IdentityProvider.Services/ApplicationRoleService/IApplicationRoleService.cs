using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityProvider.Repository.EF.Queries.UserRolesResourcesOperations.RoleOperationResource;
using IdentityProvider.Repository.EF.Queries.UserRolesResourcesOperations.UserRoleResourcesOperations;
using Microsoft.AspNet.Identity;

namespace IdentityProvider.Services.ApplicationRoleService
{
    public interface IApplicationRoleService
    {
        Task<IdentityResult> AddRoleAsync(string roleName, string optionalDescription, bool startAsNonActive = false);
        IEnumerable<UserRoleResourcesOperationsDto> FetchReasourseAndOperationsGraph();
        IEnumerable<RoleOperationResourceDto> FetchUserRoleReasourseAndOperationsGraph();
    }
}
