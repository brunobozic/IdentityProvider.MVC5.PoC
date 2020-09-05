using IdentityProvider.Services.DbSeed;
using Module.Repository.EF;
using System.Data.Entity.Migrations;

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


            try
            {
                _seeder.Seed();
            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }
}