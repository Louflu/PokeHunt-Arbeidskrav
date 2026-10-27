using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Oppgave2Pokemon.Helpers
{
    public class PasswordHelper
    {
        // Passord hasher som krypterer passordene. Her brukte jeg denne videoen fra moodle: https://youtu.be/w8I32UPEvj8 til å sette opp og har brukt
        // HMAC512 som krypteringsalgoritme
        // Dette gjør at jeg kun lagrer de krypterte passordene i databasen. 
        public static string GetHashedPassword(string password)
        {
            // Salten her genererer en byte med 64 random ints. 
            var salt = new byte[64];
            RandomNumberGenerator.Fill(salt);

            // her bruker jeg HMAC encoding til å kunne hashe passordet med salt
            using var hmac = new HMACSHA512(salt);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hash = hmac.ComputeHash(passwordBytes);

            // så konverterer jeg dette til string og legger dem sammen slik at struktur blir salt : kryptert passord+ kryptert salt
            var saltAsString = Convert.ToBase64String(salt);
            var hashAsString = Convert.ToBase64String(hash);
            return $"{saltAsString}:{hashAsString}";
        }

        public static bool VerifyPassword(string passwordRequest, string userPassword)
        {
            // her splitter den hashed passordet ved å bruke : slik at man har salt og den krypterte stringen
            var parts = userPassword.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            // deretter konverterer den salt og hashed string til byte arrays
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);

            // hvor den da med å bruke HMAC encoding krypterer password requesten med saltet og sammenlikner de krypterte stringene om de er like
            using var hmac = new HMACSHA512(salt);
            var passwordBytes = Encoding.UTF8.GetBytes(passwordRequest);
            var computedHash = hmac.ComputeHash(passwordBytes);

            var result = expectedHash.SequenceEqual(computedHash);

            return result;
        }
    }
}