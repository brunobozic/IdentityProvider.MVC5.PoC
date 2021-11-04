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
                .IsOptional();

            Property(t => t.TableName)
                .IsOptional()
                // .HasMaxLength(250)
                .IsVariableLength();

            Property(t => t.Actions)
                .IsOptional()
                // .HasMaxLength(1)
                .IsVariableLength();

            Property(t => t.UserName)
                .IsOptional();

            Property(t => t.OldData)
                .IsOptional()
                // .HasMaxLength(4000)
                .IsVariableLength();

            Property(t => t.NewData)
                .IsOptional()
                // .HasMaxLength(4000)
                .IsVariableLength();

            Property(t => t.TableIdValue)
                .IsOptional();

            Property(t => t.UpdatedAt)
                .IsOptional();

            // Table & Column Mappings
            ToTable("DbAuditTrail", "Audit");

            Property(t => t.UserId).HasColumnName("UserId");

            Property(t => t.TableName).HasColumnName("TableName");
            Property(t => t.Actions).HasColumnName("Actions");
            Property(t => t.UserName).HasColumnName("UserName");
            Property(t => t.OldData).HasColumnName("OldData");
            Property(t => t.NewData).HasColumnName("NewData");
            Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}