using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oppgave2Pokemon.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKeyUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Catches_UserId",
                table: "Catches",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catches_UserAccounts_UserId",
                table: "Catches",
                column: "UserId",
                principalTable: "UserAccounts",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catches_UserAccounts_UserId",
                table: "Catches");

            migrationBuilder.DropIndex(
                name: "IX_Catches_UserId",
                table: "Catches");
        }
    }
}
