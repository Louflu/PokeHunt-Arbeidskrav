# PokeHunt API

PokeHunt er et API som lar brukere se Pokemon database, samt opplaste fangster med respektive CP. 
I tillegg har APIet en Raid funskjon som viser brukere aktive raids og lar admins opprette/oppdatere raids.

## Eksamen: 
Her skulle jeg legge til et nytt GET endepunkt som henter inn en leaderboard med top 5 brukere utifra antall fangster.
Jeg løste det ved å bruke både linq query og for each kode til å kunne hente ut matchende data og telle i en dictionary. I tillegg fikset jeg litt på databasen
ved å endre foreign key og joinet på userID slik at jeg kunne hente inn navn fra UserAccounts til Catches.

## Oppsett

Databasen ligger i prosjektet og sender med DB. Burde bli satt opp automatisk ved oppstart. 

## Migrasjoner
Har gjort endringer i db underveis blant annet sette opp foreign keys, legge til user account db, og sette secondary type til å være nullable siden ikke alle 
Pokemon har 2 typer.

Har gjort endringer ved å legge til UserName i catches og opprette foreign Key i Catches på UserId som den får fra UserAccounts

## Kjøring
Kjøres med standard https profil fra launch settings. 

## Eksempler
# Innlogging: Admin2 med passord AdminLogin

Species:
'https://localhost:7186/species?type=Water'
https://localhost:7186/raids
curl -X 'POST' \
  'https://localhost:7186/catches' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "speciesId": 124,
  "cp": XXX,
  "caughtAtUtc": "XXX",
  "userId": 194923
}'

https://localhost:7186/leaderboards

# Kilder
https://notejoy.com/releases?id=142
https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-9.0&tabs=visual-studio
https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/crud?view=aspnetcore-9.0
https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-9.0
https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/security?view=aspnetcore-8.0
https://it-tools.tech/token-generator
https://stackoverflow.com/questions/816566/how-do-you-get-the-current-project-directory-from-c-sharp-code-when-creating-a-c
https://stackoverflow.com/questions/985651/comparing-hash-passwords
https://www.pokemon.com

# Kilder for eksamens delen:
https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=dotnet-core-cli
https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries
https://stackoverflow.com/questions/27376152/how-to-add-a-column-to-a-table-from-another-table-in-mysql
https://www.tutorialspoint.com/sqlite/sqlite_alias_syntax.htm
https://stackoverflow.com/questions/18667633/how-can-i-use-async-with-foreach

# Kilder video bilder:
https://www.entityframeworktutorial.net/code-first/what-is-code-first.aspx
https://procodeguide.com/csharp/entity-framework-code-first/
https://cheapsslweb.com/blog/why-password-salt-and-hash-make-for-better-security/
https://appcheck-ng.com/what-are-salts-and-how-do-they-improve-password-security/


# Alle prompts + svar ligger i egen fil AIPrompts:

# ChatGPT prompts:
Hva betyr dette i oppgaven min? "claims (sub, name, role)." Relatertert til authentisering i c# med API"
Krav 1) Domenemodell Lag klassene PokemonSpecies, Catch og Raid med følgende egenskaper: • PokemonSpecies: Id, Name, PokedexNumber, TypePrimary, TypeSecondary. • Catch: Id, SpeciesId (FK), Cp, CaughtAtUtc, UserId. • Raid: Id, SpeciesId (FK), GymName, Latitude, Longitude, StartsAtUtc, EndsAtUtc. • Relasjon: Én art (PokemonSpecies) kan ha mange fangster (Catch) og raids (Raid). • Bruk dataannotasjoner for validering Hvordan fungerer dataannotasjoner i csharp entity framework?"
topCatches.Where().OrderBy(c => c.Cp).Top(5).ToList(); What is wrong here?"
How do simple password hashing algorithm typically look when using HmacSha512Signature"
Kan du forklare relasjoner og Navigasjons­egenskaper litt mer basic? jeg skjønner ikke hvorfor når man arver egenskaper så må man koble opp klassene til hverandre, men her trenger man ikke det"


# Eksamen spesifikke prompts:
iqueryable async c# 
Is there any way to make this code use async/await? (kode)