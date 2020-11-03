using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCoreSqlDb.Migrations
{
    public partial class AddTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Team",
                table: "Resources",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team",
                table: "Resources");
        }
    }
}
