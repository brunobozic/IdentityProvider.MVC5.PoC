using IdentityProvider.Repository.EFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProvider.Repository.EFCore.Mapping
{
    public class AuditTrailConfiguration : IEntityTypeConfiguration<DbAuditTrail>
    {
        public void Configure(EntityTypeBuilder<DbAuditTrail> modelBuilder)
        {
            modelBuilder.ToTable("DbAuditTrail", "Audit");

            // Primary Key
            modelBuilder.HasKey(t => t.Id);

            // Since EF Core automatically treats non-nullable properties as required,
            // explicit IsRequired() calls on non-nullable types (like int, DateTime, etc.) are omitted here.

            modelBuilder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            // Removing .IsRequired(false) for nullable fields as it's the default behavior.
            // Consider uncommenting and specifying .HasMaxLength() if applicable.

            modelBuilder.Property(t => t.TableName);
            //.HasMaxLength(250);

            modelBuilder.Property(t => t.Actions);
            //.HasMaxLength(1);

            modelBuilder.Property(t => t.OldData);
            //.HasMaxLength(4000);

            modelBuilder.Property(t => t.NewData);
            //.HasMaxLength(4000);

            // Column Mappings - only specify if they differ from property names.
            // modelBuilder.Property(t => t.UserId).HasColumnName("UserId");
            // The rest of the column mappings have been omitted for brevity, as they are not necessary
            // if the column names match the property names.
        }
    }
}