using IdentityProvider.Repository.EFCore.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> modelBuilder)
        {
            modelBuilder.HasKey(e => e.Id);

            modelBuilder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Property(e => e.Name)
                .IsRequired();

            // Relationship with Resource
            modelBuilder.HasOne(i => i.Resource)
                .WithMany(i => i.Permissions)
                .IsRequired();

            // Unique Index for Permission Name
            modelBuilder.HasIndex(t => t.Name).IsUnique().HasDatabaseName("IDX_Permission_Name");
        }
    }

}