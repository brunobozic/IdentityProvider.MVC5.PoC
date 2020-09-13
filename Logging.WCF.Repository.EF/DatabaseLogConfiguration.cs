using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logging.WCF.Repository.EF
{
    public class DbLogConfiguration : IEntityTypeConfiguration<DatabaseLog>
    {
        public void Configure(EntityTypeBuilder<DatabaseLog> builder)
        {
            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties
            builder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Name);

            builder.Property(e => e.Description);


        }
    }

    //public class DatabaseLogConfiguration : EntityTypeConfiguration<DbLog>
    //{
    //    public DatabaseLogConfiguration()
    //    {
    //        // Primary Key
    //        HasKey(t => t.Id);

    //        // Properties
    //        Property(e => e.Id)
    //            .IsRequired()
    //            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

    //        Property(t => t.UserId)
    //            .IsRequired();

    //        Property(t => t.Operation)
    //            .IsOptional()
    //            // .HasMaxLength(50)
    //            .IsVariableLength();

    //        Property(t => t.CreatedDate)
    //            .IsRequired();

    //        Property(t => t.ModifiedDate)
    //            .IsRequired();

    //        Property(t => t.DeletedDate)
    //            .IsOptional();

    //        Property(t => t.Message)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.TrackingNo)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.ErrorLevel)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.InputParams)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.OutputParams)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.FileName)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.MethodName)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.LineNo)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.ColumnNo)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.AbsoluteUrl)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.ADUser)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.ClientBrowser)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.RemoteHost)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.Path)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.Query)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.Referrer)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.RequestId)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.SessionId)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.Method)
    //            .IsOptional()
    //            // .HasMaxLength(4000)
    //            .IsVariableLength();

    //        Property(t => t.ExceptionType).IsOptional()// .HasMaxLength(4000)
    //            .IsVariableLength();
    //        Property(t => t.ExceptionMessage).IsOptional()// .HasMaxLength(4000)
    //            .IsVariableLength();
    //        Property(t => t.ExceptionStackTrace).IsOptional()// .HasMaxLength(4000)
    //            .IsVariableLength();
    //        Property(t => t.InnerExceptionMessage).IsOptional()// .HasMaxLength(4000)
    //            .IsVariableLength();
    //        Property(t => t.InnerExceptionSource).IsOptional()// .HasMaxLength(4000)
    //            .IsVariableLength();
    //        Property(t => t.InnerExceptionStackTrace).IsOptional()// .HasMaxLength(4000)
    //            .IsVariableLength();
    //        Property(t => t.InnerExceptionTargetSite).IsOptional()// .HasMaxLength(4000)
    //            .IsVariableLength();
    //        Property(t => t.AssemblyQualifiedName).IsOptional()// .HasMaxLength(4000)
    //            .IsVariableLength();
    //        Property(t => t.Namespace).IsOptional()// .HasMaxLength(4000)
    //            .IsVariableLength();
    //        Property(t => t.LogSource).IsOptional()// .HasMaxLength(4000)
    //            .IsVariableLength();


    //        //this.Property(t => t.RegionDescription)
    //        //	.IsRequired()
    //        //	.IsFixedLength()
    //        //	// .HasMaxLength(50).HasColumnAnnotation(
    //        //		IndexAnnotation.AnnotationName,
    //        //		new IndexAnnotation(
    //        //			new IndexAttribute("IX_RegionDescription", 1) { IsUnique = true })); ;

    //        // Table & Column Mappings
    //        ToTable("DatabaseLog", "Log");
    //        Property(t => t.UserId).HasColumnName("UserId");
    //        Property(t => t.Operation).HasColumnName("Operation");
    //        Property(t => t.CreatedDate).HasColumnName("CreatedDate");
    //        Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
    //        Property(t => t.DeletedDate).HasColumnName("DeletedDate");
    //        Property(t => t.Message).HasColumnName("Message");
    //        Property(t => t.TrackingNo).HasColumnName("TrackingNo");
    //        Property(t => t.ErrorLevel).HasColumnName("ErrorLevel");
    //        Property(t => t.InputParams).HasColumnName("InputParams");
    //        Property(t => t.OutputParams).HasColumnName("OutputParams");
    //        Property(t => t.FileName).HasColumnName("FileName");
    //        Property(t => t.MethodName).HasColumnName("MethodName");
    //        Property(t => t.LineNo).HasColumnName("LineNo");
    //        Property(t => t.ColumnNo).HasColumnName("ColumnNo");

    //        Property(t => t.AbsoluteUrl).HasColumnName("AbsoluteUrl");
    //        Property(t => t.ADUser).HasColumnName("ADUser");
    //        Property(t => t.ClientBrowser).HasColumnName("ClientBrowser");
    //        Property(t => t.RemoteHost).HasColumnName("RemoteHost");
    //        Property(t => t.Path).HasColumnName("Path");
    //        Property(t => t.Query).HasColumnName("Query");
    //        Property(t => t.Referrer).HasColumnName("Referrer");
    //        Property(t => t.RequestId).HasColumnName("RequestId");
    //        Property(t => t.SessionId).HasColumnName("SessionId");
    //        Property(t => t.Method).HasColumnName("Method");

    //        Property(t => t.ExceptionType).HasColumnName("ExceptionType");
    //        Property(t => t.ExceptionMessage).HasColumnName("ExceptionMessage");
    //        Property(t => t.ExceptionStackTrace).HasColumnName("ExceptionStackTrace");
    //        Property(t => t.InnerExceptionMessage).HasColumnName("InnerExceptionMessage");
    //        Property(t => t.InnerExceptionSource).HasColumnName("InnerExceptionSource");
    //        Property(t => t.InnerExceptionStackTrace).HasColumnName("InnerExceptionStackTrace");
    //        Property(t => t.InnerExceptionTargetSite).HasColumnName("InnerExceptionTargetSite");
    //        Property(t => t.AssemblyQualifiedName).HasColumnName("AssemblyQualifiedName");
    //        Property(t => t.Namespace).HasColumnName("Namespace");
    //        Property(t => t.LogSource).HasColumnName("LogSource");

    //        Property(t => t.RowVersion).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
    //    }
}
