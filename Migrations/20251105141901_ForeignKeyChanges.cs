using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oppgave2Pokemon.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catches_PokemonSpecies_PokemonSpeciesId",
                table: "Catches");

            migrationBuilder.DropIndex(
                name: "IX_Catches_PokemonSpeciesId",
                table: "Catches");

            migrationBuilder.DropColumn(
                name: "PokemonSpeciesId",
                table: "Catches");

            migrationBuilder.CreateIndex(
                name: "IX_Catches_SpeciesId",
                table: "Catches",
                column: "SpeciesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catches_PokemonSpecies_SpeciesId",
                table: "Catches",
                column: "SpeciesId",
                principalTable: "PokemonSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catches_PokemonSpecies_SpeciesId",
                table: "Catches");

            migrationBuilder.DropIndex(
                name: "IX_Catches_SpeciesId",
                table: "Catches");

            migrationBuilder.AddColumn<int>(
                name: "PokemonSpeciesId",
                table: "Catches",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catches_PokemonSpeciesId",
                table: "Catches",
                column: "PokemonSpeciesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catches_PokemonSpecies_PokemonSpeciesId",
                table: "Catches",
                column: "PokemonSpeciesId",
                principalTable: "PokemonSpecies",
                principalColumn: "Id");
        }
    }
}
