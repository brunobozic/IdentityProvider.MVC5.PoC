using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace IdentityProvider.Repository.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class stuffs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeOwnsRoles_IdentityFrameworkRole_IdentityFrameworkRoleId",
                schema: "Account",
                table: "EmployeeOwnsRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleContainsGroup_IdentityFrameworkRole_IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleContainsGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleGroupContainsRole_IdentityFrameworkRole_IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleGroupContainsRole");

            migrationBuilder.DropTable(
                name: "IdentityFrameworkRole");

            migrationBuilder.DropIndex(
                name: "IX_RoleGroupContainsRole_IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleGroupContainsRole");

            migrationBuilder.DropIndex(
                name: "IX_RoleContainsGroup_IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleContainsGroup");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeOwnsRoles_IdentityFrameworkRoleId",
                schema: "Account",
                table: "EmployeeOwnsRoles");

            migrationBuilder.DropIndex(
                name: "IDX_Role_Name",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleGroupContainsRole");

            migrationBuilder.DropColumn(
                name: "IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleContainsGroup");

            migrationBuilder.DropColumn(
                name: "IdentityFrameworkRoleId",
                schema: "Account",
                table: "EmployeeOwnsRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationalUnitId",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IDX_Role_Name",
                table: "AspNetRoles",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_OrganizationalUnitId",
                table: "AspNetRoles",
                column: "OrganizationalUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_Unit_OrganizationalUnitId",
                table: "AspNetRoles",
                column: "OrganizationalUnitId",
                principalSchema: "Organization",
                principalTable: "Unit",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Unit_OrganizationalUnitId",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IDX_Role_Name",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_OrganizationalUnitId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "OrganizationalUnitId",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<string>(
                name: "IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleGroupContainsRole",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleContainsGroup",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityFrameworkRoleId",
                schema: "Account",
                table: "EmployeeOwnsRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "IdentityFrameworkRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrganizationUnitId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ActiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityFrameworkRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityFrameworkRole_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IdentityFrameworkRole_Unit_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalSchema: "Organization",
                        principalTable: "Unit",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupContainsRole_IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleGroupContainsRole",
                column: "IdentityFrameworkRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleContainsGroup_IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleContainsGroup",
                column: "IdentityFrameworkRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOwnsRoles_IdentityFrameworkRoleId",
                schema: "Account",
                table: "EmployeeOwnsRoles",
                column: "IdentityFrameworkRoleId");

            migrationBuilder.CreateIndex(
                name: "IDX_Role_Name",
                table: "AspNetRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityFrameworkRole_OrganizationUnitId",
                table: "IdentityFrameworkRole",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityFrameworkRole_UserId",
                table: "IdentityFrameworkRole",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeOwnsRoles_IdentityFrameworkRole_IdentityFrameworkRoleId",
                schema: "Account",
                table: "EmployeeOwnsRoles",
                column: "IdentityFrameworkRoleId",
                principalTable: "IdentityFrameworkRole",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleContainsGroup_IdentityFrameworkRole_IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleContainsGroup",
                column: "IdentityFrameworkRoleId",
                principalTable: "IdentityFrameworkRole",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleGroupContainsRole_IdentityFrameworkRole_IdentityFrameworkRoleId",
                schema: "Organization",
                table: "RoleGroupContainsRole",
                column: "IdentityFrameworkRoleId",
                principalTable: "IdentityFrameworkRole",
                principalColumn: "Id");
        }
    }
}
