namespace IdentityProvider.Repository.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Further_Changes_Cant_Remember_What : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Account.ResourcesHaveOperations", "OperationId", "Account.Resources");
            DropForeignKey("Account.ResourcesHaveOperations", "ResourceId", "Account.Operations");
            DropForeignKey("dbo.RoleHasResources", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RoleHasResources", "ResourceId", "Account.Resources");
            DropForeignKey("Account.RolesBelongToRoleGroups", "RoleId", "Account.RoleGroups");
            DropForeignKey("Account.RolesBelongToRoleGroups", "RoleGroupId", "dbo.AspNetRoles");
            AddForeignKey("Account.ResourcesHaveOperations", "OperationId", "Account.Resources", "Id");
            AddForeignKey("Account.ResourcesHaveOperations", "ResourceId", "Account.Operations", "Id");
            AddForeignKey("dbo.RoleHasResources", "RoleId", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.RoleHasResources", "ResourceId", "Account.Resources", "Id");
            AddForeignKey("Account.RolesBelongToRoleGroups", "RoleId", "Account.RoleGroups", "Id");
            AddForeignKey("Account.RolesBelongToRoleGroups", "RoleGroupId", "dbo.AspNetRoles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Account.RolesBelongToRoleGroups", "RoleGroupId", "dbo.AspNetRoles");
            DropForeignKey("Account.RolesBelongToRoleGroups", "RoleId", "Account.RoleGroups");
            DropForeignKey("dbo.RoleHasResources", "ResourceId", "Account.Resources");
            DropForeignKey("dbo.RoleHasResources", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("Account.ResourcesHaveOperations", "ResourceId", "Account.Operations");
            DropForeignKey("Account.ResourcesHaveOperations", "OperationId", "Account.Resources");
            AddForeignKey("Account.RolesBelongToRoleGroups", "RoleGroupId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("Account.RolesBelongToRoleGroups", "RoleId", "Account.RoleGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RoleHasResources", "ResourceId", "Account.Resources", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RoleHasResources", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("Account.ResourcesHaveOperations", "ResourceId", "Account.Operations", "Id", cascadeDelete: true);
            AddForeignKey("Account.ResourcesHaveOperations", "OperationId", "Account.Resources", "Id", cascadeDelete: true);
        }
    }
}
