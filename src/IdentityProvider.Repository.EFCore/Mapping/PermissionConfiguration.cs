using IdentityProvider.Repository.EFCore.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> modelBuilder)
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

            modelBuilder.Property(e => e.Name)

                // .HasMaxLength(100)
                .IsRequired();

            // Table & Column Mappings
            modelBuilder.Property(t => t.RowVersion)
                .IsRowVersion()
                .ValueGeneratedOnAddOrUpdate()
                ;

            modelBuilder.HasOne(i => i.Resource).WithMany(i => i.Permissions).IsRequired();

            modelBuilder.HasIndex(t => t.Name).IsUnique().HasName("IDX_Permission_Name");
        }
    }
}