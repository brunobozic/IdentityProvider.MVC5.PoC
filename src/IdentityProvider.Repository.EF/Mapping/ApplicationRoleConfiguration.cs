using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Repository.EF.Mapping
{
    public class ApplicationRoleConfiguration : EntityTypeConfiguration<ApplicationRole>
    {
        public ApplicationRoleConfiguration()
        {
            Property(t => t.Name)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_RoleName", 1) { IsUnique = true }));

            HasMany(t => t.Resources)
                .WithMany(c => c.Roles)
                .Map(m =>
                {
                    m.ToTable("RoleHasResources");
                    m.MapLeftKey("RoleId");
                    m.MapRightKey("ResourceId");
                });
        }
    }
}