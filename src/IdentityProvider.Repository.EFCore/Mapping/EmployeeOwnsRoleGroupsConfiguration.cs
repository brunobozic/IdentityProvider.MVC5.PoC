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

            // Property Configurations
            modelBuilder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            // Relationship Configurations
            modelBuilder.HasOne(e => e.Employee)
                .WithMany(b => b.RoleGroups)
                .HasForeignKey(e => e.EmployeeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Adjust the delete behavior according to your domain requirements

            modelBuilder.HasOne(e => e.RoleGroup)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.RoleGroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Adjust the delete behavior according to your domain requirements
        }
    }

}