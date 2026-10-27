using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oppgave2Pokemon.Models
{
    // Modell for Pokemonarter i databasen
    [PrimaryKey("Id")]
    public class PokemonSpecies
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PokedexNumber { get; set; }
        public string TypePrimary { get; set; } = string.Empty;
        public string? TypeSecondary { get; set; }

        // lister fra de relaterte tabellene catch og raid for relasjoner. 
        [ForeignKey ("SpeciesId")]
        public List<Catch> Catches { get; set; } = new List<Catch>();

        [ForeignKey("SpeciesId")]
        public List<Raid> Raids { get; set; } = new List<Raid>();
    }
}

// Relasjon: Én art (PokemonSpecies) kan ha mange fangster (Catch) og raids (Raid)