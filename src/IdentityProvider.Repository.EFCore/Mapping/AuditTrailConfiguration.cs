using IdentityProvider.Repository.EFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class AuditTrailConfiguration : IEntityTypeConfiguration<DbAuditTrail>
    {
        public void Configure(EntityTypeBuilder<DbAuditTrail> modelBuilder)
        {
            // Primary Key
            modelBuilder.HasKey(t => t.Id);

            // Properties
            modelBuilder.Property(e => e.Id)
                .IsRequired().ValueGeneratedOnAdd();

            modelBuilder.Property(t => t.UserId)
                .IsRequired(false);

            modelBuilder.Property(t => t.TableName)
                .IsRequired(false)
                // .HasMaxLength(250)
                ;

            modelBuilder.Property(t => t.Actions)
                .IsRequired(false)
                // .HasMaxLength(1)
                ;

            modelBuilder.Property(t => t.UserName)
                .IsRequired(false);

            modelBuilder.Property(t => t.OldData)
                .IsRequired(false)
                // .HasMaxLength(4000)
                ;

            modelBuilder.Property(t => t.NewData)
                .IsRequired(false)
                // .HasMaxLength(4000)
                ;

            modelBuilder.Property(t => t.TableIdValue)
                .IsRequired(false);

            modelBuilder.Property(t => t.UpdatedAt)
                .IsRequired(false);

            // Table & Column Mappings
            modelBuilder.ToTable("DbAuditTrail", "Audit");

            modelBuilder.Property(t => t.UserId).HasColumnName("UserId");

            modelBuilder.Property(t => t.TableName).HasColumnName("TableName");
            modelBuilder.Property(t => t.Actions).HasColumnName("Actions");
            modelBuilder.Property(t => t.UserName).HasColumnName("UserName");
            modelBuilder.Property(t => t.OldData).HasColumnName("OldData");
            modelBuilder.Property(t => t.NewData).HasColumnName("NewData");
            modelBuilder.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}