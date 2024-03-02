using IdentityProvider.Repository.EFCore.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class PermissionGroupOwnsPermissionConfiguration : IEntityTypeConfiguration<PermissionGroupOwnsPermission>
    {
        public void Configure(EntityTypeBuilder<PermissionGroupOwnsPermission> modelBuilder)
        {
            modelBuilder.HasKey(e => e.Id);

            modelBuilder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            // Establishing many-to-many relationship between PermissionGroup and Permission
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