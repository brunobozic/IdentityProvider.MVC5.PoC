using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Repository.EF.Mapping
{
    public class OrgUnitRoleGroupJoinConfiguration : EntityTypeConfiguration<OrgUnitContainsRoleGroupLink>
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
        }
    }
}
