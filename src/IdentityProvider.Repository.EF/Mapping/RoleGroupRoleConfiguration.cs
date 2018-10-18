using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Repository.EF.Mapping
{
    public class RoleGroupRoleConfiguration : EntityTypeConfiguration<RoleGroupRoleJoin>
    {
        public RoleGroupRoleConfiguration()
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

            HasRequired(ph => ph.RoleGroup)
            .WithMany(ph => ph.Roles)
            .HasForeignKey(ph => ph.RoleGroupId);

            HasRequired(ph => ph.Role)
                .WithMany(ph => ph.RoleGroups)
                .HasForeignKey(ph => ph.RoleId);
        }
    }
}
