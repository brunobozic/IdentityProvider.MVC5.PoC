namespace IdentityProvider.Repository.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Account.Employee", "Name", c => c.String());
            AddColumn("Account.Employee", "Surname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Account.Employee", "Surname");
            DropColumn("Account.Employee", "Name");
        }
    }
}
