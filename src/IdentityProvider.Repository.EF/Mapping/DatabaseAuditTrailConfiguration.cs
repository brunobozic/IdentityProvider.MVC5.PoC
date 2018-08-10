using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using IdentityProvider.Infrastructure.DatabaseAudit;

namespace IdentityProvider.Repository.EF.Mapping
{
    public class DatabaseAuditTrailConfiguration : EntityTypeConfiguration<DbAuditTrail>
    {
        public DatabaseAuditTrailConfiguration()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(e => e.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.UserId)
                .IsRequired();

            Property(t => t.CreatedDate)
                .IsRequired();

            Property(t => t.ModifiedDate)
                .IsRequired();

            Property(t => t.DeletedDate)
                .IsOptional();

            Property(t => t.TableName)
                .IsOptional()
                .HasMaxLength(250)
                .IsVariableLength();

            Property(t => t.Actions)
                .IsOptional()
                .HasMaxLength(1)
                .IsVariableLength();

            Property(t => t.UserName)
                .IsRequired();

            Property(t => t.OldData)
                .IsOptional()
                .HasMaxLength(4000)
                .IsVariableLength();

            Property(t => t.NewData)
                .IsOptional()
                .HasMaxLength(4000)
                .IsVariableLength();

            Property(t => t.TableIdValue)
                .IsOptional();

            Property(t => t.UpdatedAt)
                .IsOptional();

            //this.Property(t => t.RegionDescription)
            //	.IsRequired()
            //	.IsFixedLength()
            //	.HasMaxLength(50).HasColumnAnnotation(
            //		IndexAnnotation.AnnotationName,
            //		new IndexAnnotation(
            //			new IndexAttribute("IX_RegionDescription", 1) { IsUnique = true })); ;

            // Table & Column Mappings
            ToTable("DbAuditTrail", "Audit");
            Property(t => t.UserId).HasColumnName("UserId");
            Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            Property(t => t.DeletedDate).HasColumnName("DeletedDate");

            Property(t => t.TableName).HasColumnName("TableName");
            Property(t => t.Actions).HasColumnName("Actions");
            Property(t => t.UserName).HasColumnName("UserName");
            Property(t => t.OldData).HasColumnName("OldData");
            Property(t => t.NewData).HasColumnName("NewData");
            Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
            Property(t => t.RowVersion).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}