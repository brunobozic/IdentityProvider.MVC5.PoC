namespace IdentityProvider.Repository.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dunno : DbMigration
    {
        public override void Up()
        {
            AddColumn("Resource.Operations", "ResourcePermissionId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("Resource.Operations", "ResourcePermissionId");
        }
    }
}
