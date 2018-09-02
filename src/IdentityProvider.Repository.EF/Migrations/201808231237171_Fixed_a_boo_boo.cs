namespace IdentityProvider.Repository.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixed_a_boo_boo : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Account.ResourcesHaveOperations", name: "OperationId", newName: "__mig_tmp__0");
            RenameColumn(table: "Account.ResourcesHaveOperations", name: "ResourceId", newName: "OperationId");
            RenameColumn(table: "Account.ResourcesHaveOperations", name: "__mig_tmp__0", newName: "ResourceId");
            RenameIndex(table: "Account.ResourcesHaveOperations", name: "IX_OperationId", newName: "__mig_tmp__0");
            RenameIndex(table: "Account.ResourcesHaveOperations", name: "IX_ResourceId", newName: "IX_OperationId");
            RenameIndex(table: "Account.ResourcesHaveOperations", name: "__mig_tmp__0", newName: "IX_ResourceId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "Account.ResourcesHaveOperations", name: "IX_ResourceId", newName: "__mig_tmp__0");
            RenameIndex(table: "Account.ResourcesHaveOperations", name: "IX_OperationId", newName: "IX_ResourceId");
            RenameIndex(table: "Account.ResourcesHaveOperations", name: "__mig_tmp__0", newName: "IX_OperationId");
            RenameColumn(table: "Account.ResourcesHaveOperations", name: "ResourceId", newName: "__mig_tmp__0");
            RenameColumn(table: "Account.ResourcesHaveOperations", name: "OperationId", newName: "ResourceId");
            RenameColumn(table: "Account.ResourcesHaveOperations", name: "__mig_tmp__0", newName: "OperationId");
        }
    }
}
