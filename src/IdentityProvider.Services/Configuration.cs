using System.Data.Entity.Migrations;
using IdentityProvider.Services.DbSeed;
using Module.Repository.EF;

namespace IdentityProvider.Services
{
    //  DropCreateDatabaseAlways<SimpleMembershipDataContext>
    public class Configuration : DbMigrationsConfiguration<DataContext>
    {
        private readonly IDoSeed _seeder;

        public Configuration(IDoSeed seeder)
        {
            _seeder = seeder;
            AutomaticMigrationsEnabled = true;
            ContextKey = "HAC.Helpdesk.SimpleMembership.Repository.EF.DataContexts.DataContextAsync";
        }

        protected override void Seed(DataContext context)
        {
            _seeder.Seed();
        }
    }
}