using System.Collections.Generic;
using IdentityProvider.Repository.EF.EFDataContext;

namespace IdentityProvider.Repository.EF.Queries.UserRolesResourcesOperations.UserRoleResourcesOperations
{
    public class UserRoleResourcesOperationsQuery
    {
        private readonly AppDbContext _context;

        public UserRoleResourcesOperationsQuery(AppDbContext context)
        {
            _context = context;
        }

        public bool OnlyActiveRoles { get; set; }
        public bool OnlyActiveOperations { get; set; }
        public bool OnlyActiveResources { get; set; }
        public string RoleNameLike { get; set; }
        public string OperationNameLike { get; set; }
        public string ResourceNameLike { get; set; }

        public IEnumerable<UserRoleResourcesOperationsDto> Execute()
        {
            var identityContext = _context;
            return null;
            //return (IEnumerable<UserRoleResourcesOperationsDto>)(from resource in _context.ApplicationResource
            //                                               from operation in resource.Operations
            //                                               from roles in resource.Roles
            //                                               from cross in roles.Users
            //                                               from users in identityContext.Users
            //                                               where resource.Active
            //                                                     && operation.Active
            //                                                     && roles.Active
            //                                                     && users.Active
            //                                                     && users.Id.Equals(cross.UserId)
            //                                                     && cross.RoleId.Equals(roles.Id)
            //                                               select new UserRoleResourcesOperationsDto
            //                                               {
            //                                                   ResourceName = resource.Name,
            //                                                   RoleName = roles.Name,
            //                                                   UserName = users.UserName,
            //                                                   Operation = operation.Name,
            //                                                   User = users.UserName
            //                                               });
        }
    }
}