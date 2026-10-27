using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Oppgave2Pokemon.Helpers;
using Oppgave2Pokemon.Models;
using Oppgave2Pokemon.Models.Api;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Oppgave2Pokemon.Services
{
    // Her har jeg satt opp innlogging og Autentisering ved å bruke JWT, igjen fulgt videoen fra moodle: https://youtu.be/w8I32UPEvj8
    public class JwtService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly PokehuntContext _context;

        public JwtService(PokehuntContext dbContext, IConfiguration configuration, ILogger<JwtService> logger)
        {
            _context = dbContext;
            _configuration = configuration;
            _logger = logger;
        }

        // Metode for å generere en log in respons for den aktuelle log inn requesten
        public LoginResponse? Authenticate(LoginRequest request)
        {
            // Sjekk om requesten er gylig i UserName og/eller Passord
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password)) 
            {
                return null; 
            }

            // Verifisering som sjekker om passordet stemmer overens med den aktuelle bruker kontoen ved å bruke PasswordHelper
            var userAccount = _context.UserAccounts.FirstOrDefault(x => x.UserName == request.UserName);
            if (userAccount == null || !PasswordHelper.VerifyPassword(request.Password, userAccount.HashedPassword)) 
            {
                return null;
            }

            // Nå vet vi om bruker er gyldig og her genereres token for session og innholder også rolle som en claim for å kunne gi rettigheter
            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"] ?? throw new ArgumentNullException("Jwt Config key not present");
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);
            var claims = new Dictionary<string, object>
            {
                { "role", userAccount.Role }
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, request.UserName)
                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                Claims = claims,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponse
            {
                AccessToken = accessToken,
                UserName = request.UserName,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
            };
        }
    }
}
