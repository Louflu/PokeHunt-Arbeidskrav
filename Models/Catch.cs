using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oppgave2Pokemon.Models
{
    // Catch model til å sette opp objekter i databasen
    [PrimaryKey("Id")]
    public class Catch
    {
        public int Id { get; set; }
        public int SpeciesId { get; set; }
        public int Cp { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CaughtAtUtc { get; set; }

        //Her satte jeg denne med en feil til FK men ville ikke endre på dette i ettertid, men det er den under som er korrekt
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserAccount? User { get; set; }


        public string? UserName { get; set; }

    }
}
