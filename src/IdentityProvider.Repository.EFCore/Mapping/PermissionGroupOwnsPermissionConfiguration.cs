using IdentityProvider.Repository.EFCore.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class PermissionGroupOwnsPermissionConfiguration : IEntityTypeConfiguration<PermissionGroupOwnsPermission>
    {
        public void Configure(EntityTypeBuilder<PermissionGroupOwnsPermission> modelBuilder)
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

            modelBuilder.HasOne(bc => bc.PermissionGroup)
                .WithMany(b => b.Permissions)
                .HasForeignKey(bc => bc.PermissionGroupId)
                .IsRequired();

            modelBuilder.HasOne(bc => bc.Permission)
                .WithMany(c => c.PermissionGroups)
                .HasForeignKey(bc => bc.PermissionGroupId)
                .IsRequired();
        }
    }
}