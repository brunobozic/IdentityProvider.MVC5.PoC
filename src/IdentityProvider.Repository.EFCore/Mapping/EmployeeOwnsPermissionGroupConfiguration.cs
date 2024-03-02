using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class EmployeeOwnsPermissionGroupConfiguration : IEntityTypeConfiguration<EmployeeOwnsPermissionGroup>
    {
        public void Configure(EntityTypeBuilder<EmployeeOwnsPermissionGroup> modelBuilder)
        {
            modelBuilder.HasKey(e => e.Id);

            modelBuilder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            // Establishing clear relationships with fluent API to ensure referential integrity and navigation properties are correctly configured.
            modelBuilder.HasOne(e => e.Employee)
                .WithMany(e => e.PermissionGroups)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); // Assuming cascade delete is desired, adjust as necessary.

            modelBuilder.HasOne(e => e.PermissionGroup)
                .WithMany(pg => pg.Employees)
                .HasForeignKey(e => e.PermissionGroupId)
                .OnDelete(DeleteBehavior.Cascade); // Assuming cascade delete is desired, adjust as necessary.
        }
    }

}