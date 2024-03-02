using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class OperationConfiguration : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> modelBuilder)
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

            modelBuilder.Property(e => e.Description);

            // Unique Index for Operation Name
            modelBuilder.HasIndex(t => t.Name).IsUnique().HasDatabaseName("IDX_Operation_Name");
        }
    }

}