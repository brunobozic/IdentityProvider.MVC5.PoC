using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Repository.EF.Mapping
{
    public class EmployeeOrgUnitConfiguration : EntityTypeConfiguration<EmployeeOrgUnitJoin>
    {
        public EmployeeOrgUnitConfiguration()
        {
            // Primary Key
            HasKey(e => e.Id);

            // Properties
            Property(e => e.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            Property(t => t.RowVersion)
                .IsRowVersion()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            HasRequired(ph => ph.OrganisationalUnit)
            .WithMany(ph => ph.Employees)
            .HasForeignKey(ph => ph.EmployeeId);

            HasRequired(ph => ph.Employee)
                .WithMany(ph => ph.OrganisationalUnits)
                .HasForeignKey(ph => ph.OrganisationalUnitId);
        }
    }
}
