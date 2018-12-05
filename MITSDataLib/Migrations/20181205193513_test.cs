using Microsoft.EntityFrameworkCore.Migrations;

namespace MITSDataLib.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_WaEvents_WaEventId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "WaEventId",
                table: "Events",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Events_WaEvents_WaEventId",
                table: "Events",
                column: "WaEventId",
                principalTable: "WaEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_WaEvents_WaEventId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "WaEventId",
                table: "Events",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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
