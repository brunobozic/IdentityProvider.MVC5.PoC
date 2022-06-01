using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> modelBuilder)
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
                .IsRequired();

            // Table & Column Mappings
            modelBuilder.Property(t => t.RowVersion)
                .IsRowVersion()
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.HasIndex(t => t.Name).IsUnique().HasName("IDX_Resource_Name");
        }
    }
}