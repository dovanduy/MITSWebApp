using Microsoft.EntityFrameworkCore.Migrations;

namespace MITSDataLib.Migrations
{
    public partial class addregistrationlimitoevent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegistrationsLimit",
                table: "WaEvents",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationsLimit",
                table: "WaEvents");
        }
    }
}
