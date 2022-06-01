using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class EmployeeOwnsRoleGroupsConfiguration : IEntityTypeConfiguration<EmployeeOwnsRoleGroups>
    {
        public void Configure(EntityTypeBuilder<EmployeeOwnsRoleGroups> modelBuilder)
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

            modelBuilder.HasOne(bc => bc.Employee)
                .WithMany(b => b.RoleGroups)
                .HasForeignKey(bc => bc.EmployeeId)
                .IsRequired();

            modelBuilder.HasOne(bc => bc.RoleGroup)
                .WithMany(c => c.Employees)
                .HasForeignKey(bc => bc.RoleGroupId)
                .IsRequired();
        }
    }
}