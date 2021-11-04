SELECT * FROM [Organization].[RoleGroup] rg
LEFT JOIN [Organization].[RoleGroupContainsRoleLink] rgLi on rg.id = rgLi.RoleGroupId
LEFT JOIN [Organization].[RoleContainsPermissionGroupLink] rcPermLi on rgLi.ApplicationRoleId = rgLi.ApplicationRoleId
LEFT JOIN [Application].[PermissionGroup] pg on rcPermLi.PermissionGroupId = pg.Id
LEFT JOIN [Application].[PermissionGroupOwnsPermissionLink] permLink on pg.Id = permLink.PermissionGroupId
LEFT JOIN [Resource].[Permissions] perm on permLink.ResourcePermissionId = perm.Id


