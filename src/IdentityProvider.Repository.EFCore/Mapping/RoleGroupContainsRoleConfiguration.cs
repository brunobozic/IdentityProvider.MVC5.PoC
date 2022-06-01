using IdentityProvider.Repository.EFCore.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class RoleGroupContainsRoleConfiguration : IEntityTypeConfiguration<RoleGroupContainsRole>
    {
        public void Configure(EntityTypeBuilder<RoleGroupContainsRole> modelBuilder)
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

            modelBuilder.HasOne(bc => bc.RoleGroup)
                .WithMany(b => b.Roles)
                .HasForeignKey(bc => bc.RoleGroupId)
                .IsRequired();

            modelBuilder.HasOne(bc => bc.Role)
                .WithMany(c => c.RoleGroups)
                .HasForeignKey(bc => bc.RoleId)
                .IsRequired();
        }
    }
}