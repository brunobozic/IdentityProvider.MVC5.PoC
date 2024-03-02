using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class EmployeeOwnsRolesConfiguration : IEntityTypeConfiguration<EmployeeOwnsRoles>
    {
        public void Configure(EntityTypeBuilder<EmployeeOwnsRoles> modelBuilder)
        {
            modelBuilder.HasKey(e => e.Id);

            modelBuilder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.HasOne(bc => bc.Employee)
                .WithMany(b => b.Roles)
                .HasForeignKey(bc => bc.EmployeeId)
                .IsRequired();

            modelBuilder.HasOne(bc => bc.Role)
                .WithMany(c => c.Employees)
                .HasForeignKey(bc => bc.RoleId)
                .IsRequired();
        }
    }

}