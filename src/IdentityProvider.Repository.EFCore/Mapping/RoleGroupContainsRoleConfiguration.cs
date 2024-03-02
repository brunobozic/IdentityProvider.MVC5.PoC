using IdentityProvider.Repository.EFCore.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class RoleGroupContainsRoleConfiguration : IEntityTypeConfiguration<RoleGroupContainsRole>
    {
        public void Configure(EntityTypeBuilder<RoleGroupContainsRole> modelBuilder)
        {
            // Primary Key Configuration
            modelBuilder.HasKey(e => e.Id);

            // Property Configurations
            modelBuilder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Concurrency Token Configuration
            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            // Relationship Configurations
            // RoleGroup to RoleGroupContainsRole (One-to-Many)
            modelBuilder.HasOne(bc => bc.RoleGroup)
                .WithMany(b => b.Roles)
                .HasForeignKey(bc => bc.RoleGroupId)
                .OnDelete(DeleteBehavior.Cascade) // Ensuring cascade delete is configured, adjust based on requirements
                .IsRequired();

            // Role to RoleGroupContainsRole (One-to-Many)
            modelBuilder.HasOne(bc => bc.Role)
                .WithMany(c => c.RoleGroups)
                .HasForeignKey(bc => bc.RoleId)
                .OnDelete(DeleteBehavior.Cascade) // Ensuring cascade delete is configured, adjust based on requirements
                .IsRequired();
        }
    }

}