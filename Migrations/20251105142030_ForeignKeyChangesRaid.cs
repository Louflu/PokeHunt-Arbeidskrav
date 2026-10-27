using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oppgave2Pokemon.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyChangesRaid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Raids_PokemonSpecies_PokemonSpeciesId",
                table: "Raids");

            migrationBuilder.DropIndex(
                name: "IX_Raids_PokemonSpeciesId",
                table: "Raids");

            migrationBuilder.DropColumn(
                name: "PokemonSpeciesId",
                table: "Raids");

            migrationBuilder.CreateIndex(
                name: "IX_Raids_SpeciesId",
                table: "Raids",
                column: "SpeciesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Raids_PokemonSpecies_SpeciesId",
                table: "Raids",
                column: "SpeciesId",
                principalTable: "PokemonSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Raids_PokemonSpecies_SpeciesId",
                table: "Raids");

            migrationBuilder.DropIndex(
                name: "IX_Raids_SpeciesId",
                table: "Raids");

            migrationBuilder.AddColumn<int>(
                name: "PokemonSpeciesId",
                table: "Raids",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Raids_PokemonSpeciesId",
                table: "Raids",
                column: "PokemonSpeciesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Raids_PokemonSpecies_PokemonSpeciesId",
                table: "Raids",
                column: "PokemonSpeciesId",
                principalTable: "PokemonSpecies",
                principalColumn: "Id");
        }
    }
}
