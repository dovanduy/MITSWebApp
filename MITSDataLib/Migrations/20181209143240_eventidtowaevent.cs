using Microsoft.EntityFrameworkCore.Migrations;

namespace MITSDataLib.Migrations
{
    public partial class eventidtowaevent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_WaEvents_WaEventId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_WaEventId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "WaEventId",
                table: "Events",
                newName: "MainEventId");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "WaEvents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WaEvents_EventId",
                table: "WaEvents",
                column: "EventId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WaEvents_Events_EventId",
                table: "WaEvents",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaEvents_Events_EventId",
                table: "WaEvents");

            migrationBuilder.DropIndex(
                name: "IX_WaEvents_EventId",
                table: "WaEvents");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "WaEvents");

            migrationBuilder.RenameColumn(
                name: "MainEventId",
                table: "Events",
                newName: "WaEventId");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Events_WaEventId",
                table: "Events",
                column: "WaEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_WaEvents_WaEventId",
                table: "Events",
                column: "WaEventId",
                principalTable: "WaEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
