using IdentityProvider.Repository.EFCore.Domain.OrganizationalUnits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Module.CrossCutting;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class OrganisationalUnitConfiguration : IEntityTypeConfiguration<OrganizationalUnit>, IAuditTrail
    {
        public void Configure(EntityTypeBuilder<OrganizationalUnit> modelBuilder)
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

            modelBuilder.Property(e => e.Description)
                ;

            //Table & Column Mappings

            modelBuilder.HasIndex(t => t.Name)
                .IsUnique()
                .HasName("IDX_Organisation_Name");
        }
    }
}