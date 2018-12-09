using Microsoft.EntityFrameworkCore.Migrations;

namespace MITSDataLib.Migrations
{
    public partial class updatewaevent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_WaEvents_WaEventId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_WaRegistrationTypes_WaEvents_WaEventId",
                table: "WaRegistrationTypes");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Tags",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "WaEventId",
                table: "WaRegistrationTypes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_WaRegistrationTypes_WaEvents_WaEventId",
                table: "WaRegistrationTypes",
                column: "WaEventId",
                principalTable: "WaEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_WaEvents_WaEventId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_WaRegistrationTypes_WaEvents_WaEventId",
                table: "WaRegistrationTypes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tags",
                newName: "name");

            migrationBuilder.AlterColumn<int>(
                name: "WaEventId",
                table: "WaRegistrationTypes",
                nullable: true,
                oldClrType: typeof(int));

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

            migrationBuilder.AddForeignKey(
                name: "FK_WaRegistrationTypes_WaEvents_WaEventId",
                table: "WaRegistrationTypes",
                column: "WaEventId",
                principalTable: "WaEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
