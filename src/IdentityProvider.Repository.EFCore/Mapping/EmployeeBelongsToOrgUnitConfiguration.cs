using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class EmployeeBelongsToOrgUnitConfiguration : IEntityTypeConfiguration<EmployeeBelongsToOrgUnit>
    {
        public void Configure(EntityTypeBuilder<EmployeeBelongsToOrgUnit> modelBuilder)
        {
            modelBuilder.HasKey(t => t.Id);

            modelBuilder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            // Concurrency token configuration is streamlined and follows EF Core conventions.
            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            // Configuration for relationships is more concise, focusing on the essentials.
            modelBuilder.HasOne(bc => bc.Employee)
                .WithMany(b => b.OrganizationalUnits)
                .HasForeignKey(bc => bc.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); // Explicitly specifying the delete behavior if needed.

            modelBuilder.HasOne(bc => bc.OrganizationalUnit)
               .WithMany(c => c.Employees)
               .HasForeignKey(bc => bc.OrganizationalUnitId)
               .OnDelete(DeleteBehavior.Cascade); // Similarly, specifying delete behavior for clarity.
        }
    }

}