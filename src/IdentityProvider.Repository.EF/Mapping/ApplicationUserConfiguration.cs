
using IdentityProvider.Models.Domain.Account;
using System.Data.Entity.ModelConfiguration;

namespace IdentityProvider.Repository.EF.Mapping
{

    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            HasOptional(e => e.UserProfile)
            .WithRequired(a => a.User);

            HasOptional(e => e.Employee1)
            .WithRequired(a => a.ApplicationUser);
            //.Map(configuration => configuration.MapKey("Employee_Id"));
        }
    }
}
