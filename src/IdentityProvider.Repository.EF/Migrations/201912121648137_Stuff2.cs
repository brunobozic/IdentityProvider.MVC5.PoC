namespace IdentityProvider.Repository.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stuff2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Organization.RoleGroup", "Description", c => c.String(maxLength: 260));
            AlterColumn("Organization.Unit", "Description", c => c.String(maxLength: 260));
            AlterColumn("Application.PermissionGroup", "Description", c => c.String(maxLength: 260));
        }
        
        public override void Down()
        {
            AlterColumn("Application.PermissionGroup", "Description", c => c.String(nullable: false, maxLength: 260));
            AlterColumn("Organization.Unit", "Description", c => c.String(nullable: false, maxLength: 260));
            AlterColumn("Organization.RoleGroup", "Description", c => c.String(nullable: false, maxLength: 260));
        }
    }
}
