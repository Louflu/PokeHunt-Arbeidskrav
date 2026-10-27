using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oppgave2Pokemon.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PokemonSpecies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PokedexNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    TypePrimary = table.Column<string>(type: "TEXT", nullable: false),
                    TypeSecondary = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonSpecies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Catches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SpeciesId = table.Column<int>(type: "INTEGER", nullable: false),
                    Cp = table.Column<int>(type: "INTEGER", nullable: false),
                    CaughtAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PokemonSpeciesId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catches_PokemonSpecies_PokemonSpeciesId",
                        column: x => x.PokemonSpeciesId,
                        principalTable: "PokemonSpecies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Raids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SpeciesId = table.Column<int>(type: "INTEGER", nullable: false),
                    GymName = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    StartsAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndsAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PokemonSpeciesId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Raids_PokemonSpecies_PokemonSpeciesId",
                        column: x => x.PokemonSpeciesId,
                        principalTable: "PokemonSpecies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catches_PokemonSpeciesId",
                table: "Catches",
                column: "PokemonSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Raids_PokemonSpeciesId",
                table: "Raids",
                column: "PokemonSpeciesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catches");

            migrationBuilder.DropTable(
                name: "Raids");

            migrationBuilder.DropTable(
                name: "PokemonSpecies");
        }
    }
}
