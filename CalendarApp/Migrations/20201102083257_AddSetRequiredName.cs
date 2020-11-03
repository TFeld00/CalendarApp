using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCoreSqlDb.Migrations
{
    public partial class AddSetRequiredName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Resources",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
