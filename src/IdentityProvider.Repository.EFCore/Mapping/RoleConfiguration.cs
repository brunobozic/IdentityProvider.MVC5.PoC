using IdentityProvider.Repository.EFCore.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class RoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        [System.Obsolete]
        public void Configure(EntityTypeBuilder<AppRole> modelBuilder)
        {
            // Unique Index for Role Name
            modelBuilder
              .HasIndex(role => role.Name)
              .IsUnique()
              .HasName("IDX_Role_Name");
        }
    }

}