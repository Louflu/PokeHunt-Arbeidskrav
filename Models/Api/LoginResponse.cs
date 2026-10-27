namespace Oppgave2Pokemon.Models.Api
{
    // Login Response som gir en Accesstoken om passord er korrekt som utgår etter tid
    public class LoginResponse
    {
        public string? UserName { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }


    }
}
