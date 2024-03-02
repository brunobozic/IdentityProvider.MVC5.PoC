using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace IdentityProvider.Repository.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class stuff9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_AspNetUsers_UserId",
                schema: "Organization",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_UserId",
                schema: "Organization",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Organization",
                table: "Employee");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ApplicationUserId",
                schema: "Organization",
                table: "Employee",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_AspNetUsers_ApplicationUserId",
                schema: "Organization",
                table: "Employee",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_AspNetUsers_ApplicationUserId",
                schema: "Organization",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ApplicationUserId",
                schema: "Organization",
                table: "Employee");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "Organization",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UserId",
                schema: "Organization",
                table: "Employee",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_AspNetUsers_UserId",
                schema: "Organization",
                table: "Employee",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
