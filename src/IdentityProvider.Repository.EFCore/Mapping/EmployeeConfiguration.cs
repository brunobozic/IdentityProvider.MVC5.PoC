using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> modelBuilder)
        {
            // Primary Key
            modelBuilder.HasKey(e => e.Id);

            // Properties
            modelBuilder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd()
                ;

            // Table & Column Mappings

            // Concurrency
            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.HasOne(e => e.ApplicationUser)
                .WithOne(u => u.Employee)
                .IsRequired(false)
                //.HasForeignKey("UserId")
                ;

            
        }
    }
}