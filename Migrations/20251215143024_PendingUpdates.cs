using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oppgave2Pokemon.Migrations
{
    /// <inheritdoc />
    public partial class PendingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Catches",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Catches",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Catches",
                newName: "Username");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Catches",
                type: "nvarchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
