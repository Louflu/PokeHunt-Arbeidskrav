using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.FileSystemGlobbing;
using Oppgave2Pokemon.Models;

namespace Oppgave2Pokemon.Controllers
{
    [ApiController]
    [Route("")]
    [Authorize]
    public class PokemonController : Controller
    {
        private readonly PokehuntContext context;
        private readonly ILogger _logger;

        public PokemonController(PokehuntContext context, ILogger<PokemonController> logger)
        {
            _logger = logger;
            this.context = context;
        }

        //EKSAMEN ENDEPUNKT Oppgave B1
        [HttpGet("leaderboards/users/top-catches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        // Endepunkt som henter inn top 5 brukere utifra antall catches asynkront
        public async Task<IActionResult> GetLeaderboardTopUsers()
        {
            try
            {
                //Logger som skriver til konsoll at den starter hentingen 
                _logger.LogInformation("Fetching leaderboard");

                // Grupperer fra catches utifra UserName
                var gruppertUserId = context.Catches.GroupBy(c => c.UserName);

                var dict = new Dictionary<string, int>();

                // legger inn alle fra den gruppperte listen in i dictionary med Username som key og antall per UserName som value
                await foreach (var gruppe in gruppertUserId.AsAsyncEnumerable())
                {
                    if (gruppe.Key is not null)
                    {
                        dict.Add(gruppe.Key, gruppe.Count());
                    }
                }

                // Her henter jeg ut fra dictionary top 5 utifra value og sorterer descending
                var top5Brukere = dict.OrderByDescending(x => x.Value).Take(5);

                _logger.LogInformation("leaderboard created successfully");
                return Ok(top5Brukere);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        #region Species
        [HttpPost("species")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public IActionResult AddPokemon(PokemonSpecies species)
        {
            try
            {
                _logger.LogInformation("Uploading new Pokemon to database");
                context.PokemonSpecies.Add(species);
                context.SaveChanges();

                _logger.LogInformation("Uploaded successfully to database");
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        [HttpGet("species")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // hent alle arter og ha filtrering på ?name= og ?type=
        public IActionResult GetPokeSpecies(string? name = null, string? type = null)
        {
            try
            {
                // logger text: Fetching Pokemon + legg til antall resultater
                _logger.LogInformation("Fetching pokemon for parameters {Name}, {Type}", name, type);
                var species = context.PokemonSpecies;

                IQueryable<PokemonSpecies> filtered = species;
                if (name != null)
                {
                    filtered = filtered.Where(x => x.Name == name);
                }

                if (type != null)
                {
                    filtered = filtered.Where(x => x.TypePrimary == type || x.TypeSecondary == type);
                }

                var count = filtered.Count();
                _logger.LogInformation("Results matching search: {Count}", count);

                if (count == 0)
                {
                    return NotFound($"No results found with parameters given: {name}, {type}");
                }
                return Ok(filtered.ToList());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        [HttpPut("species")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdatePokemon(PokemonSpecies species, int id)
        {
            try
            {
                var pokemon = context.PokemonSpecies.FirstOrDefault(x => x.Id == id);

                if (pokemon != null)
                {
                    _logger.LogInformation("Pokemon found in database. Updating values:");
                    pokemon.Name = species.Name;
                    pokemon.PokedexNumber = species.PokedexNumber;
                    pokemon.TypePrimary = species.TypePrimary;
                    pokemon.TypeSecondary = species.TypeSecondary;
                    context.PokemonSpecies.Update(pokemon);
                }
                else
                {
                    _logger.LogInformation("Pokemon does not exist. Uploading new Pokemon to database ");
                    pokemon = species;
                    context.PokemonSpecies.Add(pokemon);
                }

                context.SaveChanges();
                _logger.LogInformation("Uploaded successfully to database");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        [HttpDelete("species")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePokemon(int id)
        {
            try
            {
                _logger.LogInformation("Pokemon found in database. Deleting.");
                var pokemon = context.PokemonSpecies.FirstOrDefault(x => x.Id == id);

                if (pokemon != null)
                {
                    context.PokemonSpecies.Remove(pokemon);
                    context.SaveChanges();

                    _logger.LogInformation("Removed successfully from database");
                    return Ok();
                }
                else return NotFound("No Pokemon with this ID.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }
        #endregion

        #region Raids
        [HttpPost("raids")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRaid(Raid raid)
        {
            try
            {
                _logger.LogInformation("Uploading new Raid to database");
                context.Raids.Add(raid);
                context.SaveChanges();

                _logger.LogInformation("Uploaded successfully to database");
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        [HttpGet("raids")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        //hent aktive raids
        public IActionResult GetRaids()
        {
            try
            {
                _logger.LogInformation("Fetching active raids");
                var currentTime = DateTime.UtcNow;
                var activeRaid = context.Raids;

                var raidList = activeRaid.Where(raid => raid.StartsAtUtc <= currentTime && raid.EndsAtUtc >= currentTime).ToList();

                _logger.LogInformation("There are {Count} active raids", raidList.Count);
                return Ok(raidList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        [HttpPut("raids")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRaid(Raid raid, int id)
        {
            try
            {
                var newraid = context.Raids.FirstOrDefault(x => x.Id == id);

                if (newraid != null)
                {
                    _logger.LogInformation("Raid ID found in database. Updating values:");
                    newraid.SpeciesId = raid.SpeciesId;
                    newraid.GymName = raid.GymName;
                    newraid.Latitude = raid.Latitude;
                    newraid.Longitude = raid.Longitude;
                    newraid.StartsAtUtc = raid.StartsAtUtc;
                    newraid.EndsAtUtc = raid.EndsAtUtc;
                    context.Raids.Update(newraid);
                }
                else
                {
                    _logger.LogInformation("Raid entry does not exist. Uploading new Raid to database ");
                    context.Raids.Add(raid);
                }

                context.SaveChanges();
                _logger.LogInformation("Uploaded successfully to database");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        [HttpDelete("raids")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRaid(int id)
        {
            try
            {
                _logger.LogInformation("Matching raid found in database. Deleting.");
                var raid = context.Raids.FirstOrDefault(x => x.Id == id);

                if (raid != null)
                {
                    context.Raids.Remove(raid);
                    context.SaveChanges();

                    _logger.LogInformation("Removed successfully from database");
                    return Ok();
                }
                else return NotFound("No raid found with this ID.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        #endregion

        #region Catches

        [HttpGet("catches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        // Admin fordi userid kan være sensitiv info, her ville jeg laget to endepunkter utifra claim slik at brukere også kan hente catches men ved bruk av
        // DTO hatt begrensninger for hva som blir hentet. 
        public IActionResult GetCatches(int? speciesid, int? userid)
        {
            try
            {
                _logger.LogInformation("Fetching registered catches for parameters {SpeciesId}, {UserId}", speciesid, userid);
                var caughtPokemon = context.Catches;

                IQueryable<Catch> filtered = caughtPokemon;
                if (userid != null)
                {
                    filtered = filtered.Where(x => x.UserId == userid);
                }

                if (speciesid != null)
                {
                    filtered = filtered.Where(x => x.SpeciesId == speciesid);
                }

                var count = filtered.Count();
                _logger.LogInformation("Results matching search: {Count}", count);

                if (count == 0)
                {
                    return NotFound($"No results found with parameters given: {speciesid}, {userid}");
                }
                return Ok(filtered.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        [HttpPost("catches")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "basic, Admin")]
        // registrer fangst (MÅ VÆRE INNLOGGA)
        public IActionResult PostCatch(Catch pokemoncatch)
        {
            try
            {
                _logger.LogInformation("Uploading catch to database");
                context.Catches.Add(pokemoncatch);
                context.SaveChanges();

                _logger.LogInformation("Uploaded successfully to database");
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
            
        }

        [HttpPut("catches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCatch(Catch catches, int id)
        {
            try
            {
                var newCatch = context.Catches.FirstOrDefault(x => x.Id == id);

                if (newCatch != null)
                {
                    _logger.LogInformation("Catch with matching ID found in database. Updating values:");
                    newCatch.SpeciesId = catches.SpeciesId;
                    newCatch.Cp = catches.Cp;
                    newCatch.CaughtAtUtc = catches.CaughtAtUtc;
                    newCatch.UserId = catches.UserId;
                    context.Catches.Update(newCatch);
                }
                else
                {
                    _logger.LogInformation("Catch entry does not exist. Uploading new Catch to database ");
                    context.Catches.Add(catches);
                }

                context.SaveChanges();
                _logger.LogInformation("Uploaded successfully to database");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        [HttpDelete("catches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCatch(int id)
        {
            try
            {
                _logger.LogInformation("Matching catch found in database. Deleting.");
                var caught = context.Catches.FirstOrDefault(x => x.Id == id);

                if (caught != null)
                {
                    context.Catches.Remove(caught);
                    context.SaveChanges();

                    _logger.LogInformation("Removed successfully from database");
                    return Ok();
                }
                else return NotFound("No catch found with this ID.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        #endregion


        #region Leaderboards
        [HttpGet("leaderboards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        // Leaderboard av alle fangte Pokemon utifra CP ELLER Leaderboard av trainer med Pokemon med høyest CP
        public IActionResult GetLeaderboard()
        {
            try
            {
                _logger.LogInformation("Fetching leaderboard");
                var topCatches = context.Catches.OrderByDescending(c => c.Cp).Take(5).ToList();

                // top 5 pokemon fangst (descending) sortert på cp men viser pokemon navn og CaughtAtUtc 
                var leaderboardList = new List<LeaderboardDto>();
                foreach(var pokeCatch in topCatches)
                {
                    var species = context.PokemonSpecies.First(pokemon => pokemon.Id == pokeCatch.SpeciesId);

                    var newLeaderboardObject = new LeaderboardDto()
                    {
                        Name = species.Name,
                        Cp = pokeCatch.Cp,
                        CaughtAtUtc = pokeCatch.CaughtAtUtc,
                    };
                    leaderboardList.Add(newLeaderboardObject);
                }
                _logger.LogInformation("Leaderboard created successfully");
                return Ok(leaderboardList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
        }

        
        #endregion
    }
    public class LeaderboardDto
    {
        public required string Name { get; set; }
        public int Cp { get; set; }
        public DateTime CaughtAtUtc { get; set;}
    }
}
