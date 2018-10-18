namespace IdentityProvider.Repository.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Log.DatabaseLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Operation = c.String(maxLength: 50),
                        UserId = c.Int(nullable: false),
                        UserName = c.String(),
                        Message = c.String(maxLength: 4000),
                        TrackingNo = c.String(maxLength: 4000),
                        ErrorLevel = c.String(maxLength: 4000),
                        InputParams = c.String(maxLength: 4000),
                        OutputParams = c.String(maxLength: 4000),
                        FileName = c.String(maxLength: 4000),
                        MethodName = c.String(maxLength: 4000),
                        LineNo = c.String(maxLength: 4000),
                        ColumnNo = c.String(maxLength: 4000),
                        AbsoluteUrl = c.String(maxLength: 4000),
                        ADUser = c.String(maxLength: 4000),
                        ClientBrowser = c.String(maxLength: 4000),
                        RemoteHost = c.String(maxLength: 4000),
                        Path = c.String(maxLength: 4000),
                        Query = c.String(maxLength: 4000),
                        Referrer = c.String(maxLength: 4000),
                        RequestId = c.String(maxLength: 4000),
                        SessionId = c.String(maxLength: 4000),
                        Method = c.String(maxLength: 4000),
                        ExceptionType = c.String(maxLength: 4000),
                        ExceptionMessage = c.String(maxLength: 4000),
                        ExceptionStackTrace = c.String(maxLength: 4000),
                        InnerExceptionMessage = c.String(maxLength: 4000),
                        InnerExceptionSource = c.String(maxLength: 4000),
                        InnerExceptionStackTrace = c.String(maxLength: 4000),
                        InnerExceptionTargetSite = c.String(maxLength: 4000),
                        AssemblyQualifiedName = c.String(maxLength: 4000),
                        Namespace = c.String(maxLength: 4000),
                        LogSource = c.String(maxLength: 4000),
                        TimeStamp = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Audit.DbAuditTrail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableName = c.String(maxLength: 250),
                        UserId = c.Int(),
                        UserName = c.String(),
                        OldData = c.String(maxLength: 4000),
                        NewData = c.String(maxLength: 4000),
                        TableIdValue = c.Long(),
                        UpdatedAt = c.DateTime(),
                        Actions = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Account.Operations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        Active = c.Boolean(nullable: false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_OperationName");
            
            CreateTable(
                "Account.Resources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Active = c.Boolean(nullable: false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_ResourceName");
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
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
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        UserProfile_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserProfile_Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex")
                .Index(t => t.Name, unique: true, name: "IX_RoleName")
                .Index(t => t.UserProfile_Id);
            
            CreateTable(
                "Account.RoleGroupRoleJoin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        RoleGroupId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("Account.RoleGroups", t => t.RoleGroupId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.RoleGroupId);
            
            CreateTable(
                "Account.RoleGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 260),
                        Active = c.Boolean(nullable: false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_RoleGroupName");
            
            CreateTable(
                "Account.OrgUnitRoleGroupJoin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganisationalUnitId = c.Int(nullable: false),
                        RoleGroupId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(nullable: false),
                        OrganisationalUnit_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrganisationalUnits", t => t.OrganisationalUnit_Id)
                .ForeignKey("dbo.OrganisationalUnits", t => t.RoleGroupId, cascadeDelete: true)
                .ForeignKey("Account.RoleGroups", t => t.OrganisationalUnitId, cascadeDelete: true)
                .Index(t => t.OrganisationalUnitId)
                .Index(t => t.RoleGroupId)
                .Index(t => t.OrganisationalUnit_Id);
            
            CreateTable(
                "dbo.OrganisationalUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 100),
                        SecurityWeight = c.Int(nullable: false),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_OrganisationalUnitName");
            
            CreateTable(
                "Account.EmployeeOrgUnitJoin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganisationalUnitId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Account.Employee", t => t.OrganisationalUnitId, cascadeDelete: true)
                .ForeignKey("dbo.OrganisationalUnits", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.OrganisationalUnitId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "Account.Employee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsConfirmed = c.Boolean(nullable: false),
                        ConfirmationToken = c.String(),
                        LastName = c.String(),
                        FirstName = c.String(),
                        UserImage = c.Binary(),
                        MobilePhone = c.String(),
                        HomePhone = c.String(),
                        LastLoginDate = c.DateTime(),
                        TwoFactorSecret = c.String(),
                        UserUid = c.Guid(nullable: false),
                        PasswordResetToken = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        TrackingState = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Account.UserProfile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfilePicture = c.Binary(),
                        Active = c.Boolean(nullable: false),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(),
                        DeletedById = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsDeleted = c.Boolean(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "Account.ResourcesHaveOperations",
                c => new
                    {
                        ResourceId = c.Int(nullable: false),
                        OperationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ResourceId, t.OperationId })
                .ForeignKey("Account.Resources", t => t.ResourceId)
                .ForeignKey("Account.Operations", t => t.OperationId)
                .Index(t => t.ResourceId)
                .Index(t => t.OperationId);
            
            CreateTable(
                "dbo.RoleHasResources",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        ResourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.ResourceId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("Account.Resources", t => t.ResourceId)
                .Index(t => t.RoleId)
                .Index(t => t.ResourceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("Account.RoleGroupRoleJoin", "RoleGroupId", "Account.RoleGroups");
            DropForeignKey("Account.OrgUnitRoleGroupJoin", "OrganisationalUnitId", "Account.RoleGroups");
            DropForeignKey("Account.OrgUnitRoleGroupJoin", "RoleGroupId", "dbo.OrganisationalUnits");
            DropForeignKey("Account.OrgUnitRoleGroupJoin", "OrganisationalUnit_Id", "dbo.OrganisationalUnits");
            DropForeignKey("Account.EmployeeOrgUnitJoin", "EmployeeId", "dbo.OrganisationalUnits");
            DropForeignKey("Account.EmployeeOrgUnitJoin", "OrganisationalUnitId", "Account.Employee");
            DropForeignKey("Account.UserProfile", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetRoles", "UserProfile_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("Account.Employee", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("Account.RoleGroupRoleJoin", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RoleHasResources", "ResourceId", "Account.Resources");
            DropForeignKey("dbo.RoleHasResources", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("Account.ResourcesHaveOperations", "OperationId", "Account.Operations");
            DropForeignKey("Account.ResourcesHaveOperations", "ResourceId", "Account.Resources");
            DropIndex("dbo.RoleHasResources", new[] { "ResourceId" });
            DropIndex("dbo.RoleHasResources", new[] { "RoleId" });
            DropIndex("Account.ResourcesHaveOperations", new[] { "OperationId" });
            DropIndex("Account.ResourcesHaveOperations", new[] { "ResourceId" });
            DropIndex("Account.UserProfile", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("Account.Employee", new[] { "ApplicationUser_Id" });
            DropIndex("Account.EmployeeOrgUnitJoin", new[] { "EmployeeId" });
            DropIndex("Account.EmployeeOrgUnitJoin", new[] { "OrganisationalUnitId" });
            DropIndex("dbo.OrganisationalUnits", "IX_OrganisationalUnitName");
            DropIndex("Account.OrgUnitRoleGroupJoin", new[] { "OrganisationalUnit_Id" });
            DropIndex("Account.OrgUnitRoleGroupJoin", new[] { "RoleGroupId" });
            DropIndex("Account.OrgUnitRoleGroupJoin", new[] { "OrganisationalUnitId" });
            DropIndex("Account.RoleGroups", "IX_RoleGroupName");
            DropIndex("Account.RoleGroupRoleJoin", new[] { "RoleGroupId" });
            DropIndex("Account.RoleGroupRoleJoin", new[] { "RoleId" });
            DropIndex("dbo.AspNetRoles", new[] { "UserProfile_Id" });
            DropIndex("dbo.AspNetRoles", "IX_RoleName");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("Account.Resources", "IX_ResourceName");
            DropIndex("Account.Operations", "IX_OperationName");
            DropTable("dbo.RoleHasResources");
            DropTable("Account.ResourcesHaveOperations");
            DropTable("Account.UserProfile");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("Account.Employee");
            DropTable("Account.EmployeeOrgUnitJoin");
            DropTable("dbo.OrganisationalUnits");
            DropTable("Account.OrgUnitRoleGroupJoin");
            DropTable("Account.RoleGroups");
            DropTable("Account.RoleGroupRoleJoin");
            DropTable("dbo.AspNetRoles");
            DropTable("Account.Resources");
            DropTable("Account.Operations");
            DropTable("Audit.DbAuditTrail");
            DropTable("Log.DatabaseLog");
        }
    }
}
