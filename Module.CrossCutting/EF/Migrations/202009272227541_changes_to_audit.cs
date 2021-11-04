using System.Data.Entity.Migrations;

namespace IdentityProvider.Repository.EF.Migrations
{
    public partial class changes_to_audit : DbMigration
    {
        public override void Up()
        {
            DropTable("Log.DatabaseLog");
        }

        public override void Down()
        {
            CreateTable(
                    "Log.DatabaseLog",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Operation = c.String(),
                        UserId = c.Int(false),
                        UserName = c.String(),
                        Message = c.String(),
                        TrackingNo = c.String(),
                        ErrorLevel = c.String(),
                        InputParams = c.String(),
                        OutputParams = c.String(),
                        FileName = c.String(),
                        MethodName = c.String(),
                        LineNo = c.String(),
                        ColumnNo = c.String(),
                        AbsoluteUrl = c.String(),
                        ADUser = c.String(),
                        ClientBrowser = c.String(),
                        RemoteHost = c.String(),
                        Path = c.String(),
                        Query = c.String(),
                        Referrer = c.String(),
                        RequestId = c.String(),
                        SessionId = c.String(),
                        Method = c.String(),
                        ExceptionType = c.String(),
                        ExceptionMessage = c.String(),
                        ExceptionStackTrace = c.String(),
                        InnerExceptionMessage = c.String(),
                        InnerExceptionSource = c.String(),
                        InnerExceptionStackTrace = c.String(),
                        InnerExceptionTargetSite = c.String(),
                        AssemblyQualifiedName = c.String(),
                        Namespace = c.String(),
                        LogSource = c.String(),
                        TimeStamp = c.DateTime(false),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(false),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(false),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id);
        }
    }
}