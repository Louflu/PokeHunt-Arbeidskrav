using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oppgave2Pokemon.Helpers;
using Oppgave2Pokemon.Models;
using Oppgave2Pokemon.Models.Api;
using Oppgave2Pokemon.Services;
using System.Xml.Linq;

namespace Oppgave2Pokemon.Controllers
{
    // Controller som har endepunktene for oppretting av bruker og innlogging av bruker
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly PokehuntContext context;
        private readonly ILogger _logger;  

        public AuthController(JwtService jwtservice, PokehuntContext context, ILogger<AuthController> logger)
        {
            _jwtService = jwtservice;
            this.context = context;
            _logger = logger;
        }
        
        
        [HttpPost("register")]
        public IActionResult RegisterUser(string username, string password)
        {
            // ta inn username + passord + assign rolle basic og userid + lagres i database

            try
            {
                _logger.LogInformation("Attempting to register new user with {UserName} and Password", username);
                var newUser = new UserAccount { UserName = username, HashedPassword = PasswordHelper.GetHashedPassword(password), Role = "basic" };
                context.UserAccounts.Add(newUser);
                context.SaveChanges();

                _logger.LogInformation("User registered successfully");
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest("Something went wrong");
            }
            
        }

        [HttpPost("login")]
        // claims (sub, name, role).
        public IActionResult LoginUser(string username, string password)
        {
            var loginRequest = new LoginRequest { UserName = username, Password = password };
            var response = _jwtService.Authenticate(loginRequest);

            _logger.LogInformation("Attempting to log in using {UserName} and Password", username);
            if (response != null)
            {
                _logger.LogInformation("User logged in successfully");
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Failed attempt at logging in.");
                return BadRequest("Login failed. Wrong password or user does not exist");
            }
        }
    }
}
