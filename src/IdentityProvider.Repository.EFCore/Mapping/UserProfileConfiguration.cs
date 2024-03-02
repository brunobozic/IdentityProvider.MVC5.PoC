using IdentityProvider.Repository.EFCore.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> modelBuilder)
        {
            // Primary Key
            modelBuilder.HasKey(e => e.Id);

            // Property Configurations
            modelBuilder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Concurrency Token
            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            // One-To-One Relationship Configuration
            modelBuilder.HasOne(up => up.User)
                .WithOne(u => u.UserProfile)
                .HasForeignKey<ApplicationUser>(u => u.UserProfileId)
                .IsRequired(false);
        }
    }




}