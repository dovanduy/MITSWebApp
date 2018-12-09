using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MITSDataLib.Migrations
{
    public partial class changewaregistrationsname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaRegistrationTypes");

            migrationBuilder.CreateTable(
                name: "WaRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WaId = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CodeRequired = table.Column<bool>(nullable: false),
                    RegistrationCode = table.Column<string>(nullable: true),
                    AvailableFrom = table.Column<DateTime>(nullable: false),
                    AvailableThrough = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    WaEventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaRegistrations_WaEvents_WaEventId",
                        column: x => x.WaEventId,
                        principalTable: "WaEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaRegistrations_WaEventId",
                table: "WaRegistrations",
                column: "WaEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaRegistrations");

            migrationBuilder.CreateTable(
                name: "WaRegistrationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvailableFrom = table.Column<DateTime>(nullable: false),
                    AvailableThrough = table.Column<DateTime>(nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CodeRequired = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RegistrationCode = table.Column<string>(nullable: true),
                    WaEventId = table.Column<int>(nullable: false),
                    WaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaRegistrationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaRegistrationTypes_WaEvents_WaEventId",
                        column: x => x.WaEventId,
                        principalTable: "WaEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaRegistrationTypes_WaEventId",
                table: "WaRegistrationTypes",
                column: "WaEventId");
        }
    }
}
