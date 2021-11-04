using System.Collections.Generic;
using IdentityProvider.Repository.EF.EFDataContext;
using System.Linq;
using System.Data.Entity;
using IdentityProvider.Models.ViewModels.Permissions;

namespace IdentityProvider.Repository.EF.Queries.UserRolesResourcesOperations.RoleOperationResource
{
    public class RoleOperationResourceQuery : ISimpleQueryObject<RoleOperationResourceDto>
    {
        private readonly AppDbContext _context;

        public RoleOperationResourceQuery(AppDbContext context)
        {
            _context = context;
        }

        public bool OnlyActiveRoles { get; set; }
        public bool OnlyActiveOperations { get; set; }
        public bool OnlyActiveResources { get; set; }
        public string RoleNameLike { get; set; }
        public string OperationNameLike { get; set; }
        public string ResourceNameLike { get; set; }

        public IEnumerable<RoleOperationResourceDto> Execute()
        {

            return (IEnumerable<RoleOperationResourceDto>)(from roleHasPermissionGroups in _context.RoleContainsPermissionGroupLink
                                                           join role in _context.Roles
                                                            on roleHasPermissionGroups.ApplicationRoleId equals role.Id
                                                           join resourcePermissionGroup in _context.ResourcePermissionGroup
                                                            on roleHasPermissionGroups.PermissionGroupId equals resourcePermissionGroup.Id
                                                           join resourcePermissionLink in _context.PermissionGroupOwnsPermissionLink
                                                           on resourcePermissionGroup.Id equals resourcePermissionLink.PermissionGroupId
                                                           join resourcePermission in _context.Permission
                                                            on resourcePermissionLink.ResourcePermissionId equals resourcePermission.Id
                                                           join resource in _context.ApplicationResource
                                                            on resourcePermission.ApplicationResourceId equals resource.Id
                                                           join operation in _context.Operation
                                                            on resourcePermission.OperationId equals operation.Id

                                                           where resource.Active
                                                                 && operation.Active

                                                           group resourcePermissionLink by new
                                                           {
                                                               PermissionGroup = resourcePermissionLink.PermissionGroup,
                                                               PermissionLink = resourcePermissionLink.Permission
                                                           }
                                                           into groupedByPermissionGroup // grouping by PermissionGroup
                                                           // let stuff = groupedByPermissionGroup.OrderByDescending(g => g.CreatedDate) // ordering PermissionGroup by date created
                                                           //select new // projection into an anonymous type whilst obeying the previous grouping and ordering
                                                           //{
                                                           //    GroupName = groupedByPermissionGroup.Key.PermissionGroup.Name
                                                           //    // FIlteredFindingStatuses = stuff.FirstOrDefault(), // taking top one newest finding status, after having ordered it by date descending
                                                           //}
                                                           select new RoleOperationResourceDto
                                                           {
                                                               //Group = new PermissionGroupDto
                                                               //{
                                                               //    Name = groupedByPermissionGroup.Key.PermissionGroup.Name,
                                                               //    Permissions = new PermissionDto { }

                                                               //},
                                                           }).ToListAsync();
        }
    }
}