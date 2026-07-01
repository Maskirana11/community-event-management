using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunityEvents.Migrations
{
    /// <inheritdoc />
    public partial class LinkUserAccountToParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParticipantId",
                table: "UserAccounts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_ParticipantId",
                table: "UserAccounts",
                column: "ParticipantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Participants_ParticipantId",
                table: "UserAccounts",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Participants_ParticipantId",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_ParticipantId",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "UserAccounts");
        }
    }
}
