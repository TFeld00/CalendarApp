using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCoreSqlDb.Migrations
{
    public partial class RenameTeamName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Team",
                newName: "TeamName",
                table: "Resources");

            //migrationBuilder.AddColumn<string>(
            //    name: "TeamName",
            //    table: "Resources",
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeamName",
                newName: "Team",
                table: "Resources");

            //migrationBuilder.AddColumn<string>(
            //    name: "Team",
            //    table: "Resources",
            //    type: "nvarchar(max)",
            //    nullable: true);
        }
    }
}
