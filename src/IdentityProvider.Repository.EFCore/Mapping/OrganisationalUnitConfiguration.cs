using IdentityProvider.Repository.EFCore.Domain.OrganizationalUnits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class OrganisationalUnitConfiguration : IEntityTypeConfiguration<OrganizationalUnit>
    {
        public void Configure(EntityTypeBuilder<OrganizationalUnit> modelBuilder)
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

            // Unique Index for Organization Name
            modelBuilder.HasIndex(t => t.Name).IsUnique().HasDatabaseName("IDX_Organisation_Name");
        }
    }

}