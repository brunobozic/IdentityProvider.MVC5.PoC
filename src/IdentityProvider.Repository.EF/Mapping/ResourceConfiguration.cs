using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Repository.EF.Mapping
{
    public class ResourceConfiguration : EntityTypeConfiguration<Resource>
    {
        public ResourceConfiguration()
        {
            // Primary Key
            HasKey(e => e.Id);

            // Properties
            Property(e => e.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Name)
                .IsVariableLength()
                .HasMaxLength(100)
                .IsRequired();

            // Table & Column Mappings
            Property(t => t.RowVersion)
                .IsRowVersion()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(t => t.Name)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_ResourceName", 1) { IsUnique = true }));

            HasMany<Operation>(t => t.Operations)
                .WithMany(c => c.Resources)
                .Map(m =>
                {
                    m.ToTable("ResourcesHaveOperations", "Account");
                    m.MapLeftKey("OperationId");
                    m.MapRightKey("ResourceId");
                });
        }
    }
}