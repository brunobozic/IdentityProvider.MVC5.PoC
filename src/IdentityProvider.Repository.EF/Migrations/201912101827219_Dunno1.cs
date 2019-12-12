namespace IdentityProvider.Repository.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dunno1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("Resource.Operations", "ResourcePermissionId");
        }
        
        public override void Down()
        {
            AddColumn("Resource.Operations", "ResourcePermissionId", c => c.Int());
        }
    }
}
