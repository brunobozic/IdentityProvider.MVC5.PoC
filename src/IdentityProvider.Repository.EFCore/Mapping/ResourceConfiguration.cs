using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        [System.Obsolete]
        public void Configure(EntityTypeBuilder<Resource> modelBuilder)
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

            // Required Name with Unique Index
            modelBuilder.Property(e => e.Name)
                .IsRequired();

            // Table & Column Mappings
            // The RowVersion is automatically configured as a concurrency token; explicit mapping to column is not required unless customizing.
            // Removed explicit RowVersion configuration here as it's handled by the IsConcurrencyToken() call above.

            // Unique Index for Name
            modelBuilder
                 .HasIndex(t => t.Name)
                 .IsUnique()
                 .HasName("IDX_Resource_Name");

        }
    }

}