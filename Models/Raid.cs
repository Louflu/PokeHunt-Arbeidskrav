using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Oppgave2Pokemon.Models
{
    // Modell for Raid tabellen i databasen
    [PrimaryKey("Id")]
    public class Raid
    {
        public int Id { get; set; }
        public int SpeciesId { get; set; }
        public string GymName { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartsAtUtc { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndsAtUtc { get; set; }
    }
}