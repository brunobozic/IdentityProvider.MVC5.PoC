using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> modelBuilder)
        {
            // Simplified key configuration with assumptions on EF Core's conventions.
            modelBuilder.HasKey(e => e.Id);

            // Streamlining property configuration with EF Core's convention-based approach.
            modelBuilder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            // Configuration for concurrency token to ensure data integrity.
            modelBuilder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            // Refined relationship configuration to ensure clarity and enforce constraints.
            modelBuilder.HasOne(e => e.ApplicationUser)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.ApplicationUserId) // Correcting the foreign key property name
                .IsRequired(false) // Assuming optional relationship
                .OnDelete(DeleteBehavior.ClientSetNull); // Choosing an appropriate delete behavior

        }
    }

}