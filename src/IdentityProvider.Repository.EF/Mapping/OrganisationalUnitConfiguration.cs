using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using IdentityProvider.Infrastructure;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Repository.EF.Mapping
{
    public class OrganisationalUnitConfiguration : EntityTypeConfiguration<OrganizationalUnit>, IAuditTrail
    {
        public OrganisationalUnitConfiguration()
        {
            //Primary Key
            HasKey(e => e.Id);

            //Properties
            Property(e => e.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Name)
                .IsVariableLength()
                // .HasMaxLength(100)
                .IsRequired();

            Property(e => e.Description)
                .IsVariableLength()
                // .HasMaxLength(100)
                ;

            //Table & Column Mappings
            Property(t => t.RowVersion)
                .IsRowVersion()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(t => t.Name)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName ,
                    new IndexAnnotation(
                        new IndexAttribute("IX_OrganisationalUnitName" , 1) { IsUnique = true }));

        }
    }
}
