using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCoreSqlDb.Migrations
{
    public partial class RenameIconToBadge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasIcon",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "Icon",
                newName: "Badge",
                table: "Teams");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Badge",
                newName: "Icon",
                table: "Teams");

            migrationBuilder.AddColumn<bool>(
                name: "HasIcon",
                table: "Teams",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
