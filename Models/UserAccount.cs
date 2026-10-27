using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Oppgave2Pokemon.Models
{
    // Modell for UserAccount i databasen som lagrer den krypterte versjonen av passordet
    [PrimaryKey("UserId")]
    public class UserAccount
    {
        public string? UserName { get; set; }

    // Sørger for at hvis man henter ut useraccounts så får man ikke med den krypterte passordet for sikkerhet
        [JsonIgnore]
        public string? HashedPassword { get; set; }
        public int UserId { get; set; }
        public string? Role { get; set; }

    }
}
