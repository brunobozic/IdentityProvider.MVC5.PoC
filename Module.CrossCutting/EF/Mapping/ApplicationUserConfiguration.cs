using System.Data.Entity.ModelConfiguration;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Repository.EF.Mapping
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            HasOptional(e => e.UserProfile)
                .WithRequired(a => a.User);

            HasOptional(e => e.Employee)
                .WithRequired(a => a.ApplicationUser);
            //.Map(configuration => configuration.MapKey("Employee_Id"));
        }
    }
}