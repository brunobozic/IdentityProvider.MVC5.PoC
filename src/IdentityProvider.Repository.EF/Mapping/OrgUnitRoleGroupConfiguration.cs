using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Repository.EF.Mapping
{
    public class OrgUnitRoleGroupJoinConfiguration : EntityTypeConfiguration<OrgUnitRoleGroupJoin>
    {
        public OrgUnitRoleGroupJoinConfiguration()
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
            .WithMany(ph => ph.RoleGroups)
            .HasForeignKey(ph => ph.RoleGroupId);

            HasRequired(ph => ph.RoleGroup)
                .WithMany(ph => ph.OrganisationalUnits)
                .HasForeignKey(ph => ph.OrganisationalUnitId);
        }
    }
}
