using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Logging.WCF.Repository.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "DbLog",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(nullable: false),
                    ActiveTo = table.Column<DateTimeOffset>(nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    DateModified = table.Column<DateTimeOffset>(nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedBy = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Operation = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    TrackingNo = table.Column<string>(nullable: true),
                    ErrorLevel = table.Column<string>(nullable: true),
                    InputParams = table.Column<string>(nullable: true),
                    OutputParams = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    MethodName = table.Column<string>(nullable: true),
                    LineNo = table.Column<string>(nullable: true),
                    ColumnNo = table.Column<string>(nullable: true),
                    AbsoluteUrl = table.Column<string>(nullable: true),
                    ADUser = table.Column<string>(nullable: true),
                    ClientBrowser = table.Column<string>(nullable: true),
                    RemoteHost = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Query = table.Column<string>(nullable: true),
                    Referrer = table.Column<string>(nullable: true),
                    RequestId = table.Column<string>(nullable: true),
                    SessionId = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    ExceptionType = table.Column<string>(nullable: true),
                    ExceptionMessage = table.Column<string>(nullable: true),
                    ExceptionStackTrace = table.Column<string>(nullable: true),
                    InnerExceptionMessage = table.Column<string>(nullable: true),
                    InnerExceptionSource = table.Column<string>(nullable: true),
                    InnerExceptionStackTrace = table.Column<string>(nullable: true),
                    InnerExceptionTargetSite = table.Column<string>(nullable: true),
                    AssemblyQualifiedName = table.Column<string>(nullable: true),
                    Namespace = table.Column<string>(nullable: true),
                    LogSource = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_DbLog", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "DbLog");
        }
    }
}