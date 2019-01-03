using Microsoft.EntityFrameworkCore.Migrations;

namespace MITSDataLib.Migrations
{
    public partial class updateeventcheckinaduittable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheckInAt",
                table: "EventCheckInAudits",
                newName: "CheckedInAt");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EventCheckInAudits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "EventCheckInAudits");

            migrationBuilder.RenameColumn(
                name: "CheckedInAt",
                table: "EventCheckInAudits",
                newName: "CheckInAt");
        }
    }
}
