using Microsoft.EntityFrameworkCore.Migrations;

namespace MITSDataLib.Migrations
{
    public partial class addeventtypetoeventtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSponsor",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "EventRegistrationType",
                table: "Events",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventRegistrationType",
                table: "Events");

            migrationBuilder.AddColumn<bool>(
                name: "IsSponsor",
                table: "Events",
                nullable: false,
                defaultValue: false);
        }
    }
}
