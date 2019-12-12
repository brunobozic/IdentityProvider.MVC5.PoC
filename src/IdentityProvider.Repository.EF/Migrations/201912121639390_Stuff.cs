namespace IdentityProvider.Repository.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stuff : DbMigration
    {
        public override void Up()
        {
            DropIndex("Application.Resources", "IX_ApplicationResourceName");
            DropIndex("Resource.Permissions", "IX_ResourcePermissionName");
            DropIndex("Resource.Operations", "IX_OperationName");
            DropIndex("Organization.RoleGroup", "IX_RoleGroupName");
            DropIndex("Organization.Unit", "IX_OrganisationalUnitName");
            AlterColumn("Application.Resources", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Application.Resources", "Description", c => c.String(maxLength: 260));
            AlterColumn("Resource.Permissions", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Resource.Permissions", "Description", c => c.String(maxLength: 260));
            AlterColumn("Resource.Operations", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Resource.Operations", "Description", c => c.String(maxLength: 260));
            AlterColumn("Log.DatabaseLog", "Operation", c => c.String());
            AlterColumn("Log.DatabaseLog", "Message", c => c.String());
            AlterColumn("Log.DatabaseLog", "TrackingNo", c => c.String());
            AlterColumn("Log.DatabaseLog", "ErrorLevel", c => c.String());
            AlterColumn("Log.DatabaseLog", "InputParams", c => c.String());
            AlterColumn("Log.DatabaseLog", "OutputParams", c => c.String());
            AlterColumn("Log.DatabaseLog", "FileName", c => c.String());
            AlterColumn("Log.DatabaseLog", "MethodName", c => c.String());
            AlterColumn("Log.DatabaseLog", "LineNo", c => c.String());
            AlterColumn("Log.DatabaseLog", "ColumnNo", c => c.String());
            AlterColumn("Log.DatabaseLog", "AbsoluteUrl", c => c.String());
            AlterColumn("Log.DatabaseLog", "ADUser", c => c.String());
            AlterColumn("Log.DatabaseLog", "ClientBrowser", c => c.String());
            AlterColumn("Log.DatabaseLog", "RemoteHost", c => c.String());
            AlterColumn("Log.DatabaseLog", "Path", c => c.String());
            AlterColumn("Log.DatabaseLog", "Query", c => c.String());
            AlterColumn("Log.DatabaseLog", "Referrer", c => c.String());
            AlterColumn("Log.DatabaseLog", "RequestId", c => c.String());
            AlterColumn("Log.DatabaseLog", "SessionId", c => c.String());
            AlterColumn("Log.DatabaseLog", "Method", c => c.String());
            AlterColumn("Log.DatabaseLog", "ExceptionType", c => c.String());
            AlterColumn("Log.DatabaseLog", "ExceptionMessage", c => c.String());
            AlterColumn("Log.DatabaseLog", "ExceptionStackTrace", c => c.String());
            AlterColumn("Log.DatabaseLog", "InnerExceptionMessage", c => c.String());
            AlterColumn("Log.DatabaseLog", "InnerExceptionSource", c => c.String());
            AlterColumn("Log.DatabaseLog", "InnerExceptionStackTrace", c => c.String());
            AlterColumn("Log.DatabaseLog", "InnerExceptionTargetSite", c => c.String());
            AlterColumn("Log.DatabaseLog", "AssemblyQualifiedName", c => c.String());
            AlterColumn("Log.DatabaseLog", "Namespace", c => c.String());
            AlterColumn("Log.DatabaseLog", "LogSource", c => c.String());
            AlterColumn("Audit.DbAuditTrail", "TableName", c => c.String());
            AlterColumn("Audit.DbAuditTrail", "OldData", c => c.String());
            AlterColumn("Audit.DbAuditTrail", "NewData", c => c.String());
            AlterColumn("Audit.DbAuditTrail", "Actions", c => c.String());
            AlterColumn("dbo.AspNetRoles", "Description", c => c.String(maxLength: 260));
            AlterColumn("Organization.RoleGroup", "Name", c => c.String(nullable: false, maxLength: 260));
            AlterColumn("Organization.Unit", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Organization.Unit", "Description", c => c.String(nullable: false, maxLength: 260));
            AlterColumn("Application.PermissionGroup", "Name", c => c.String(nullable: false, maxLength: 260));
            CreateIndex("Application.Resources", "Name", unique: true, name: "IX_ApplicationResourceName");
            CreateIndex("Resource.Permissions", "Name", unique: true, name: "IX_ResourcePermissionName");
            CreateIndex("Resource.Operations", "Name", unique: true, name: "IX_OperationName");
            CreateIndex("Organization.RoleGroup", "Name", unique: true, name: "IX_RoleGroupName");
            CreateIndex("Organization.Unit", "Name", unique: true, name: "IX_OrganisationalUnitName");
        }
        
        public override void Down()
        {
            DropIndex("Organization.Unit", "IX_OrganisationalUnitName");
            DropIndex("Organization.RoleGroup", "IX_RoleGroupName");
            DropIndex("Resource.Operations", "IX_OperationName");
            DropIndex("Resource.Permissions", "IX_ResourcePermissionName");
            DropIndex("Application.Resources", "IX_ApplicationResourceName");
            AlterColumn("Application.PermissionGroup", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Organization.Unit", "Description", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Organization.Unit", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Organization.RoleGroup", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.AspNetRoles", "Description", c => c.String());
            AlterColumn("Audit.DbAuditTrail", "Actions", c => c.String(maxLength: 1));
            AlterColumn("Audit.DbAuditTrail", "NewData", c => c.String(maxLength: 4000));
            AlterColumn("Audit.DbAuditTrail", "OldData", c => c.String(maxLength: 4000));
            AlterColumn("Audit.DbAuditTrail", "TableName", c => c.String(maxLength: 250));
            AlterColumn("Log.DatabaseLog", "LogSource", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "Namespace", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "AssemblyQualifiedName", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "InnerExceptionTargetSite", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "InnerExceptionStackTrace", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "InnerExceptionSource", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "InnerExceptionMessage", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "ExceptionStackTrace", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "ExceptionMessage", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "ExceptionType", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "Method", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "SessionId", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "RequestId", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "Referrer", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "Query", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "Path", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "RemoteHost", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "ClientBrowser", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "ADUser", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "AbsoluteUrl", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "ColumnNo", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "LineNo", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "MethodName", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "FileName", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "OutputParams", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "InputParams", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "ErrorLevel", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "TrackingNo", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "Message", c => c.String(maxLength: 4000));
            AlterColumn("Log.DatabaseLog", "Operation", c => c.String(maxLength: 50));
            AlterColumn("Resource.Operations", "Description", c => c.String(maxLength: 100));
            AlterColumn("Resource.Operations", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Resource.Permissions", "Description", c => c.String());
            AlterColumn("Resource.Permissions", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Application.Resources", "Description", c => c.String(nullable: false));
            AlterColumn("Application.Resources", "Name", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("Organization.Unit", "Name", unique: true, name: "IX_OrganisationalUnitName");
            CreateIndex("Organization.RoleGroup", "Name", unique: true, name: "IX_RoleGroupName");
            CreateIndex("Resource.Operations", "Name", unique: true, name: "IX_OperationName");
            CreateIndex("Resource.Permissions", "Name", unique: true, name: "IX_ResourcePermissionName");
            CreateIndex("Application.Resources", "Name", unique: true, name: "IX_ApplicationResourceName");
        }
    }
}
