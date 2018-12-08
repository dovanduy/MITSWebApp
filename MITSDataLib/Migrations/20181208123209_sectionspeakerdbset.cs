using Microsoft.EntityFrameworkCore.Migrations;

namespace MITSDataLib.Migrations
{
    public partial class sectionspeakerdbset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionSpeaker_Sections_SectionId",
                table: "SectionSpeaker");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionSpeaker_Speakers_SpeakerId",
                table: "SectionSpeaker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SectionSpeaker",
                table: "SectionSpeaker");

            migrationBuilder.RenameTable(
                name: "SectionSpeaker",
                newName: "SectionsSpeakers");

            migrationBuilder.RenameIndex(
                name: "IX_SectionSpeaker_SpeakerId",
                table: "SectionsSpeakers",
                newName: "IX_SectionsSpeakers_SpeakerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SectionsSpeakers",
                table: "SectionsSpeakers",
                columns: new[] { "SectionId", "SpeakerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SectionsSpeakers_Sections_SectionId",
                table: "SectionsSpeakers",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionsSpeakers_Speakers_SpeakerId",
                table: "SectionsSpeakers",
                column: "SpeakerId",
                principalTable: "Speakers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionsSpeakers_Sections_SectionId",
                table: "SectionsSpeakers");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionsSpeakers_Speakers_SpeakerId",
                table: "SectionsSpeakers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SectionsSpeakers",
                table: "SectionsSpeakers");

            migrationBuilder.RenameTable(
                name: "SectionsSpeakers",
                newName: "SectionSpeaker");

            migrationBuilder.RenameIndex(
                name: "IX_SectionsSpeakers_SpeakerId",
                table: "SectionSpeaker",
                newName: "IX_SectionSpeaker_SpeakerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SectionSpeaker",
                table: "SectionSpeaker",
                columns: new[] { "SectionId", "SpeakerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SectionSpeaker_Sections_SectionId",
                table: "SectionSpeaker",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionSpeaker_Speakers_SpeakerId",
                table: "SectionSpeaker",
                column: "SpeakerId",
                principalTable: "Speakers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
