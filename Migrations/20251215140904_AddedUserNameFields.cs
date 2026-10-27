using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oppgave2Pokemon.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Catches",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Catches");
        }
    }
}
