using IdentityProvider.Repository.EFCore.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> modelBuilder)
        {
            modelBuilder.HasOne(e => e.UserProfile)
                .WithOne(a => a.User)
                .IsRequired(false);

            modelBuilder.HasOne(e => e.Employee)
                .WithOne(a => a.ApplicationUser)
                // .HasForeignKey("EmployeeId")
                .IsRequired(false);

            // Each User can have many UserClaims
            modelBuilder.HasMany(e => e.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            modelBuilder.HasMany(e => e.Logins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            modelBuilder.HasMany(e => e.Tokens)
                .WithOne(e => e.User)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            modelBuilder.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        }
    }
}