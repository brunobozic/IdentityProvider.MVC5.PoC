using IdentityProvider.Repository.EFCore.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class RoleContainsPermissionsConfiguration : IEntityTypeConfiguration<RoleContainsPermissions>
    {
        public void Configure(EntityTypeBuilder<RoleContainsPermissions> modelBuilder)
        {
            // Primary Key
            modelBuilder.HasKey(e => e.Id);

            // Properties
            modelBuilder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd()
                ;

            // Concurrency
            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.HasOne(bc => bc.Role)
                .WithMany(b => b.Permissions)
                .HasForeignKey(bc => bc.RoleId)
                .IsRequired();

            modelBuilder.HasOne(bc => bc.Permission)
                .WithMany(c => c.Roles)
                .HasForeignKey(bc => bc.PermissionId)
                .IsRequired();
        }
    }
}