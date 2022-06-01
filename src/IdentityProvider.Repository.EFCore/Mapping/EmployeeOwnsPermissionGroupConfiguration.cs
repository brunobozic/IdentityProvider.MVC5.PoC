using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class EmployeeOwnsPermissionGroupConfiguration : IEntityTypeConfiguration<EmployeeOwnsPermissionGroup>
    {
        public void Configure(EntityTypeBuilder<EmployeeOwnsPermissionGroup> modelBuilder)
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
                .WithMany(b => b.PermissionGroups)
                .HasForeignKey(bc => bc.EmployeeId)
                .IsRequired();

            modelBuilder.HasOne(bc => bc.PermissionGroup)
                .WithMany(c => c.Employees)
                .HasForeignKey(bc => bc.PermissionGroupId)
                .IsRequired();
        }
    }
}