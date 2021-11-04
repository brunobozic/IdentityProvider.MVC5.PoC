namespace IdentityProvider.Repository.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Application.ResourcePermissions", newName: "Permissions");
            RenameTable(name: "Organization.ResourcePermissionGroup", newName: "PermissionGroup");
            RenameTable(name: "Organization.RoleContainsResourcePermissionGroupLink", newName: "RoleContainsPermissionGroupLink");
            MoveTable(name: "Application.Permissions", newSchema: "Resource");
            MoveTable(name: "Organization.PermissionGroupOwnsPermissionLink", newSchema: "Application");
            MoveTable(name: "Organization.PermissionGroup", newSchema: "Application");
            DropForeignKey("Application.ResourcePermissions", "ApplicationResource_Id", "Application.Resources");
            DropForeignKey("Application.ResourcePermissions", "Operation_Id", "Resource.Operations");
            DropForeignKey("Organization.PermissionGroupOwnsPermissionLink", "ResourcePermissionId", "Application.ResourcePermissions");
            DropForeignKey("Organization.RoleContainsResourcePermissionGroupLink", "ResourcePermissionGroup_Id", "Organization.ResourcePermissionGroup");
            DropIndex("Resource.Permissions", new[] { "ApplicationResource_Id" });
            DropIndex("Resource.Permissions", new[] { "Operation_Id" });
            DropIndex("Application.PermissionGroupOwnsPermissionLink", new[] { "ResourcePermissionId" });
            DropIndex("Organization.RoleContainsPermissionGroupLink", new[] { "ResourcePermissionGroup_Id" });
            RenameColumn(table: "Resource.Permissions", name: "ApplicationResource_Id", newName: "ApplicationResourceId");
            RenameColumn(table: "Resource.Permissions", name: "Operation_Id", newName: "OperationId");
            RenameColumn(table: "Organization.RoleGroupContainsRoleLink", name: "RoleId", newName: "ApplicationRoleId");
            RenameColumn(table: "Organization.RoleContainsPermissionGroupLink", name: "ResourcePermissionGroup_Id", newName: "PermissionGroupId");
            RenameIndex(table: "Organization.RoleGroupContainsRoleLink", name: "IX_RoleId", newName: "IX_ApplicationRoleId");
            AddColumn("Application.Resources", "MakeActive", c => c.Boolean(nullable: false));
            AddColumn("Application.Resources", "ActiveUntil", c => c.DateTime());
            AddColumn("Audit.DbAuditTrail", "TrackingState", c => c.Int(nullable: false));
            AddColumn("Audit.DbAuditTrail", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("Application.PermissionGroupOwnsPermissionLink", "Permission_Id", c => c.Int());
            AlterColumn("Application.Resources", "Description", c => c.String(nullable: false));
            AlterColumn("Resource.Permissions", "ApplicationResourceId", c => c.Int(nullable: false));
            AlterColumn("Resource.Permissions", "OperationId", c => c.Int(nullable: false));
            AlterColumn("Organization.RoleContainsPermissionGroupLink", "PermissionGroupId", c => c.Int(nullable: false));
            CreateIndex("Resource.Permissions", "ApplicationResourceId");
            CreateIndex("Resource.Permissions", "OperationId");
            CreateIndex("Application.PermissionGroupOwnsPermissionLink", "Permission_Id");
            CreateIndex("Organization.RoleContainsPermissionGroupLink", "PermissionGroupId");
            AddForeignKey("Resource.Permissions", "ApplicationResourceId", "Application.Resources", "Id", cascadeDelete: true);
            AddForeignKey("Resource.Permissions", "OperationId", "Resource.Operations", "Id", cascadeDelete: true);
            AddForeignKey("Application.PermissionGroupOwnsPermissionLink", "Permission_Id", "Resource.Permissions", "Id");
            AddForeignKey("Organization.RoleContainsPermissionGroupLink", "PermissionGroupId", "Application.PermissionGroup", "Id", cascadeDelete: true);
            DropColumn("Organization.RoleContainsPermissionGroupLink", "ResourcePermissionId");
        }
        
        public override void Down()
        {
            AddColumn("Organization.RoleContainsPermissionGroupLink", "ResourcePermissionId", c => c.Int(nullable: false));
            DropForeignKey("Organization.RoleContainsPermissionGroupLink", "PermissionGroupId", "Application.PermissionGroup");
            DropForeignKey("Application.PermissionGroupOwnsPermissionLink", "Permission_Id", "Resource.Permissions");
            DropForeignKey("Resource.Permissions", "OperationId", "Resource.Operations");
            DropForeignKey("Resource.Permissions", "ApplicationResourceId", "Application.Resources");
            DropIndex("Organization.RoleContainsPermissionGroupLink", new[] { "PermissionGroupId" });
            DropIndex("Application.PermissionGroupOwnsPermissionLink", new[] { "Permission_Id" });
            DropIndex("Resource.Permissions", new[] { "OperationId" });
            DropIndex("Resource.Permissions", new[] { "ApplicationResourceId" });
            AlterColumn("Organization.RoleContainsPermissionGroupLink", "PermissionGroupId", c => c.Int());
            AlterColumn("Resource.Permissions", "OperationId", c => c.Int());
            AlterColumn("Resource.Permissions", "ApplicationResourceId", c => c.Int());
            AlterColumn("Application.Resources", "Description", c => c.String());
            DropColumn("Application.PermissionGroupOwnsPermissionLink", "Permission_Id");
            DropColumn("Audit.DbAuditTrail", "IsDeleted");
            DropColumn("Audit.DbAuditTrail", "TrackingState");
            DropColumn("Application.Resources", "ActiveUntil");
            DropColumn("Application.Resources", "MakeActive");
            RenameIndex(table: "Organization.RoleGroupContainsRoleLink", name: "IX_ApplicationRoleId", newName: "IX_RoleId");
            RenameColumn(table: "Organization.RoleContainsPermissionGroupLink", name: "PermissionGroupId", newName: "ResourcePermissionGroup_Id");
            RenameColumn(table: "Organization.RoleGroupContainsRoleLink", name: "ApplicationRoleId", newName: "RoleId");
            RenameColumn(table: "Resource.Permissions", name: "OperationId", newName: "Operation_Id");
            RenameColumn(table: "Resource.Permissions", name: "ApplicationResourceId", newName: "ApplicationResource_Id");
            CreateIndex("Organization.RoleContainsPermissionGroupLink", "ResourcePermissionGroup_Id");
            CreateIndex("Application.PermissionGroupOwnsPermissionLink", "ResourcePermissionId");
            CreateIndex("Resource.Permissions", "Operation_Id");
            CreateIndex("Resource.Permissions", "ApplicationResource_Id");
            AddForeignKey("Organization.RoleContainsResourcePermissionGroupLink", "ResourcePermissionGroup_Id", "Organization.ResourcePermissionGroup", "Id");
            AddForeignKey("Organization.PermissionGroupOwnsPermissionLink", "ResourcePermissionId", "Application.ResourcePermissions", "Id", cascadeDelete: true);
            AddForeignKey("Application.ResourcePermissions", "Operation_Id", "Resource.Operations", "Id");
            AddForeignKey("Application.ResourcePermissions", "ApplicationResource_Id", "Application.Resources", "Id");
            MoveTable(name: "Application.PermissionGroup", newSchema: "Organization");
            MoveTable(name: "Application.PermissionGroupOwnsPermissionLink", newSchema: "Organization");
            MoveTable(name: "Resource.Permissions", newSchema: "Application");
            RenameTable(name: "Organization.RoleContainsPermissionGroupLink", newName: "RoleContainsResourcePermissionGroupLink");
            RenameTable(name: "Organization.PermissionGroup", newName: "ResourcePermissionGroup");
            RenameTable(name: "Application.Permissions", newName: "ResourcePermissions");
        }
    }
}
