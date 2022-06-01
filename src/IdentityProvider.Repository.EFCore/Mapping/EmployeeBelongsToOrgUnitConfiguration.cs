using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class EmployeeBelongsToOrgUnitConfiguration : IEntityTypeConfiguration<EmployeeBelongsToOrgUnit>
    {
        public void Configure(EntityTypeBuilder<EmployeeBelongsToOrgUnit> modelBuilder)
        {
            // Primary Key
            modelBuilder.HasKey(t => t.Id);

            // Properties
            modelBuilder.Property(e => e.Id)
                .IsRequired().ValueGeneratedOnAdd();

            // Concurrency
            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.HasOne(bc => bc.Employee)
                .WithMany(b => b.OrganizationalUnits)
                .HasForeignKey(bc => bc.EmployeeId)
                .IsRequired();

            modelBuilder.HasOne(bc => bc.OrganizationalUnit)
               .WithMany(c => c.Employees)
               .HasForeignKey(bc => bc.OrganizationalUnitId)
               .IsRequired();
        }
    }
}