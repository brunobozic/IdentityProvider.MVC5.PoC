using System.Data.Entity.Migrations;

namespace IdentityProvider.Repository.EF.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "Application.Resources",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(false, 50),
                        Description = c.String(maxLength: 260),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        MakeActive = c.Boolean(false),
                        ActiveUntil = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_ApplicationResourceName");

            CreateTable(
                    "Resource.Permissions",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(false, 50),
                        Description = c.String(maxLength: 260),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ApplicationResourceId = c.Int(false),
                        OperationId = c.Int(false),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Application.Resources", t => t.ApplicationResourceId, true)
                .ForeignKey("Resource.Operations", t => t.OperationId, true)
                .Index(t => t.Name, unique: true, name: "IX_ResourcePermissionName")
                .Index(t => t.ApplicationResourceId)
                .Index(t => t.OperationId);

            CreateTable(
                    "Resource.Operations",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(false, 50),
                        Description = c.String(maxLength: 260),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_OperationName");

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

            CreateTable(
                    "Audit.DbAuditTrail",
                    c => new
                    {
                        Id = c.Int(false, true),
                        TableName = c.String(),
                        UserId = c.Int(),
                        UserName = c.String(),
                        OldData = c.String(),
                        NewData = c.String(),
                        TableIdValue = c.Long(),
                        UpdatedAt = c.DateTime(),
                        Actions = c.String(),
                        TrackingState = c.Int(false),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "Organization.Employee",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        Name = c.String(),
                        Surname = c.String(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false),
                        ApplicationUser_Id = c.String(false, 128)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);

            CreateTable(
                    "dbo.AspNetUsers",
                    c => new
                    {
                        Id = c.String(false, 128),
                        IsConfirmed = c.Boolean(false),
                        ConfirmationToken = c.String(),
                        LastName = c.String(),
                        FirstName = c.String(),
                        UserImage = c.Binary(),
                        MobilePhone = c.String(),
                        HomePhone = c.String(),
                        LastLoginDate = c.DateTime(),
                        TwoFactorSecret = c.String(),
                        UserUid = c.Guid(false),
                        PasswordResetToken = c.String(),
                        IsDeleted = c.Boolean(false),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        TrackingState = c.Int(false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(false),
                        TwoFactorEnabled = c.Boolean(false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(false),
                        AccessFailedCount = c.Int(false),
                        UserName = c.String(false, 256)
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                    "dbo.AspNetUserClaims",
                    c => new
                    {
                        Id = c.Int(false, true),
                        UserId = c.String(false, 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, true)
                .Index(t => t.UserId);

            CreateTable(
                    "dbo.AspNetUserLogins",
                    c => new
                    {
                        LoginProvider = c.String(false, 128),
                        ProviderKey = c.String(false, 128),
                        UserId = c.String(false, 128)
                    })
                .PrimaryKey(t => new {t.LoginProvider, t.ProviderKey, t.UserId})
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, true)
                .Index(t => t.UserId);

            CreateTable(
                    "dbo.AspNetRoles",
                    c => new
                    {
                        Id = c.String(false, 128),
                        Name = c.String(false, 256),
                        Description = c.String(maxLength: 260),
                        TrackingState = c.Int(),
                        Active = c.Boolean(),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        IsDeleted = c.Boolean(),
                        RowVersion = c.Binary(),
                        Discriminator = c.String(false, 128),
                        UserProfile_Id = c.String(maxLength: 128)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserProfile_Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex")
                .Index(t => t.Name, unique: true, name: "IX_ApplicationRoleName")
                .Index(t => t.UserProfile_Id);

            CreateTable(
                    "Organization.RoleGroupContainsRoleLink",
                    c => new
                    {
                        Id = c.Int(false, true),
                        ApplicationRoleId = c.String(maxLength: 128),
                        RoleGroupId = c.Int(false),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetRoles", t => t.ApplicationRoleId)
                .ForeignKey("Organization.RoleGroup", t => t.RoleGroupId, true)
                .Index(t => t.ApplicationRoleId)
                .Index(t => t.RoleGroupId);

            CreateTable(
                    "Organization.RoleGroup",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(false, 260),
                        Description = c.String(maxLength: 260),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_RoleGroupName");

            CreateTable(
                    "Organization.OrgUnitContainsRoleGroupLink",
                    c => new
                    {
                        Id = c.Int(false, true),
                        OrganizationalUnitId = c.Int(false),
                        RoleGroupId = c.Int(false),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false),
                        OrganizationalUnit_Id = c.Int(),
                        OrganizationalUnit_Id1 = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Organization.Unit", t => t.OrganizationalUnit_Id)
                .ForeignKey("Organization.Unit", t => t.OrganizationalUnit_Id1)
                .ForeignKey("Organization.Unit", t => t.OrganizationalUnitId, true)
                .ForeignKey("Organization.RoleGroup", t => t.RoleGroupId, true)
                .Index(t => t.OrganizationalUnitId)
                .Index(t => t.RoleGroupId)
                .Index(t => t.OrganizationalUnit_Id)
                .Index(t => t.OrganizationalUnit_Id1);

            CreateTable(
                    "Organization.Unit",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        Name = c.String(false, 50),
                        Description = c.String(maxLength: 260),
                        SecurityWeight = c.Int(false),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_OrganisationalUnitName");

            CreateTable(
                    "Organization.EmployeeBelongsToOrgUnitLink",
                    c => new
                    {
                        Id = c.Int(false, true),
                        OrganizationalUnitId = c.Int(false),
                        EmployeeId = c.Int(false),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Organization.Employee", t => t.EmployeeId, true)
                .ForeignKey("Organization.Unit", t => t.OrganizationalUnitId, true)
                .Index(t => t.OrganizationalUnitId)
                .Index(t => t.EmployeeId);

            CreateTable(
                    "dbo.AspNetUserRoles",
                    c => new
                    {
                        UserId = c.String(false, 128),
                        RoleId = c.String(false, 128)
                    })
                .PrimaryKey(t => new {t.UserId, t.RoleId})
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                    "Account.UserProfile",
                    c => new
                    {
                        Id = c.Int(false, true),
                        ProfilePicture = c.Binary(),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false),
                        User_Id = c.String(false, 128)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);

            CreateTable(
                    "Organization.OrgUnitContainsRoleLink",
                    c => new
                    {
                        Id = c.Int(false, true),
                        OrganizationalUnitId = c.Int(false),
                        RoleId = c.String(maxLength: 128),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Organization.Unit", t => t.OrganizationalUnitId, true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.OrganizationalUnitId)
                .Index(t => t.RoleId);

            CreateTable(
                    "Application.PermissionGroupOwnsPermissionLink",
                    c => new
                    {
                        Id = c.Int(false, true),
                        ResourcePermissionId = c.Int(false),
                        PermissionGroupId = c.Int(false),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false),
                        Permission_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Resource.Permissions", t => t.Permission_Id)
                .ForeignKey("Application.PermissionGroup", t => t.PermissionGroupId, true)
                .Index(t => t.PermissionGroupId)
                .Index(t => t.Permission_Id);

            CreateTable(
                    "Application.PermissionGroup",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(false, 260),
                        Description = c.String(maxLength: 260),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "Organization.RoleContainsPermissionGroupLink",
                    c => new
                    {
                        Id = c.Int(false, true),
                        ApplicationRoleId = c.String(maxLength: 128),
                        PermissionGroupId = c.Int(false),
                        Active = c.Boolean(false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetRoles", t => t.ApplicationRoleId)
                .ForeignKey("Application.PermissionGroup", t => t.PermissionGroupId, true)
                .Index(t => t.ApplicationRoleId)
                .Index(t => t.PermissionGroupId);
        }

        public override void Down()
        {
            DropForeignKey("Organization.RoleContainsPermissionGroupLink", "PermissionGroupId",
                "Application.PermissionGroup");
            DropForeignKey("Organization.RoleContainsPermissionGroupLink", "ApplicationRoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("Application.PermissionGroupOwnsPermissionLink", "PermissionGroupId",
                "Application.PermissionGroup");
            DropForeignKey("Application.PermissionGroupOwnsPermissionLink", "Permission_Id", "Resource.Permissions");
            DropForeignKey("Organization.OrgUnitContainsRoleLink", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("Organization.OrgUnitContainsRoleLink", "OrganizationalUnitId", "Organization.Unit");
            DropForeignKey("Organization.Employee", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("Account.UserProfile", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetRoles", "UserProfile_Id", "dbo.AspNetUsers");
            DropForeignKey("Organization.RoleGroupContainsRoleLink", "RoleGroupId", "Organization.RoleGroup");
            DropForeignKey("Organization.OrgUnitContainsRoleGroupLink", "RoleGroupId", "Organization.RoleGroup");
            DropForeignKey("Organization.OrgUnitContainsRoleGroupLink", "OrganizationalUnitId", "Organization.Unit");
            DropForeignKey("Organization.OrgUnitContainsRoleGroupLink", "OrganizationalUnit_Id1", "Organization.Unit");
            DropForeignKey("Organization.OrgUnitContainsRoleGroupLink", "OrganizationalUnit_Id", "Organization.Unit");
            DropForeignKey("Organization.EmployeeBelongsToOrgUnitLink", "OrganizationalUnitId", "Organization.Unit");
            DropForeignKey("Organization.EmployeeBelongsToOrgUnitLink", "EmployeeId", "Organization.Employee");
            DropForeignKey("Organization.RoleGroupContainsRoleLink", "ApplicationRoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("Resource.Permissions", "OperationId", "Resource.Operations");
            DropForeignKey("Resource.Permissions", "ApplicationResourceId", "Application.Resources");
            DropIndex("Organization.RoleContainsPermissionGroupLink", new[] {"PermissionGroupId"});
            DropIndex("Organization.RoleContainsPermissionGroupLink", new[] {"ApplicationRoleId"});
            DropIndex("Application.PermissionGroupOwnsPermissionLink", new[] {"Permission_Id"});
            DropIndex("Application.PermissionGroupOwnsPermissionLink", new[] {"PermissionGroupId"});
            DropIndex("Organization.OrgUnitContainsRoleLink", new[] {"RoleId"});
            DropIndex("Organization.OrgUnitContainsRoleLink", new[] {"OrganizationalUnitId"});
            DropIndex("Account.UserProfile", new[] {"User_Id"});
            DropIndex("dbo.AspNetUserRoles", new[] {"RoleId"});
            DropIndex("dbo.AspNetUserRoles", new[] {"UserId"});
            DropIndex("Organization.EmployeeBelongsToOrgUnitLink", new[] {"EmployeeId"});
            DropIndex("Organization.EmployeeBelongsToOrgUnitLink", new[] {"OrganizationalUnitId"});
            DropIndex("Organization.Unit", "IX_OrganisationalUnitName");
            DropIndex("Organization.OrgUnitContainsRoleGroupLink", new[] {"OrganizationalUnit_Id1"});
            DropIndex("Organization.OrgUnitContainsRoleGroupLink", new[] {"OrganizationalUnit_Id"});
            DropIndex("Organization.OrgUnitContainsRoleGroupLink", new[] {"RoleGroupId"});
            DropIndex("Organization.OrgUnitContainsRoleGroupLink", new[] {"OrganizationalUnitId"});
            DropIndex("Organization.RoleGroup", "IX_RoleGroupName");
            DropIndex("Organization.RoleGroupContainsRoleLink", new[] {"RoleGroupId"});
            DropIndex("Organization.RoleGroupContainsRoleLink", new[] {"ApplicationRoleId"});
            DropIndex("dbo.AspNetRoles", new[] {"UserProfile_Id"});
            DropIndex("dbo.AspNetRoles", "IX_ApplicationRoleName");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserLogins", new[] {"UserId"});
            DropIndex("dbo.AspNetUserClaims", new[] {"UserId"});
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("Organization.Employee", new[] {"ApplicationUser_Id"});
            DropIndex("Resource.Operations", "IX_OperationName");
            DropIndex("Resource.Permissions", new[] {"OperationId"});
            DropIndex("Resource.Permissions", new[] {"ApplicationResourceId"});
            DropIndex("Resource.Permissions", "IX_ResourcePermissionName");
            DropIndex("Application.Resources", "IX_ApplicationResourceName");
            DropTable("Organization.RoleContainsPermissionGroupLink");
            DropTable("Application.PermissionGroup");
            DropTable("Application.PermissionGroupOwnsPermissionLink");
            DropTable("Organization.OrgUnitContainsRoleLink");
            DropTable("Account.UserProfile");
            DropTable("dbo.AspNetUserRoles");
            DropTable("Organization.EmployeeBelongsToOrgUnitLink");
            DropTable("Organization.Unit");
            DropTable("Organization.OrgUnitContainsRoleGroupLink");
            DropTable("Organization.RoleGroup");
            DropTable("Organization.RoleGroupContainsRoleLink");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("Organization.Employee");
            DropTable("Audit.DbAuditTrail");
            DropTable("Log.DatabaseLog");
            DropTable("Resource.Operations");
            DropTable("Resource.Permissions");
            DropTable("Application.Resources");
        }
    }
}