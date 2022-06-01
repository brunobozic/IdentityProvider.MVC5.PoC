using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityProvider.Repository.EFCore.Migrations
{
    public partial class Fixed_rogue_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IDX_Employee_Name",
                schema: "Organization",
                table: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Organization",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Organization",
                table: "Employee",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IDX_Employee_Name",
                schema: "Organization",
                table: "Employee",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}
