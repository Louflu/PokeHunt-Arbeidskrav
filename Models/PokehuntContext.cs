using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Oppgave2Pokemon.Models
{
    // Entity framework kontekst model for databasen 
    public class PokehuntContext : DbContext
    {
        public DbSet<Catch> Catches { get; set; }
        public DbSet<Raid> Raids { get; set; }
        public DbSet<PokemonSpecies> PokemonSpecies { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        public string DbPath { get; }

        public PokehuntContext()
        {
            string workingDirectory = Environment.CurrentDirectory;
            DbPath = System.IO.Path.Join(workingDirectory, "pokehunt.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}
