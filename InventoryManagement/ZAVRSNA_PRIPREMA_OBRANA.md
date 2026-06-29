# Zavrsna priprema za obranu i razumijevanje koda

Ovaj dokument sluzi kao kratki vodic za obranu projekta InventoryManagement. Cilj nije uciti tekst napamet, nego znati objasniti sto aplikacija radi, gdje se u kodu nalaze glavni dijelovi i kako dokazati da funkcionalnosti rade.

## Kratko predstavljanje projekta

InventoryManagement je ASP.NET Core MVC aplikacija za upravljanje inventarom. Aplikacija prikazuje i odrzava proizvode, kategorije, dobavljace, skladista, stanje zaliha, narudzbe, stavke narudzbi i poslovne korisnike.

Projekt ima tri glavna nacina rada:

- web sucelje kroz MVC controllere i Razor viewove
- REST API kroz `/api/...` controllere
- integracijske testove koji provjeravaju CRUD ponasanje, pretragu, logiranje i fallback za AI savjete

U obrani je najvaznije pokazati da razumijes tok podataka: zahtjev dolazi u controller, controller koristi repozitorij ili DbContext, podaci se citaju ili mijenjaju u bazi, a rezultat se vraca kao HTML view ili JSON odgovor.

## Arhitektura u jednoj minuti

| Dio | Datoteke | Sto radi | Kako to objasniti |
| --- | --- | --- | --- |
| Pokretanje aplikacije | `InventoryManagement/Program.cs` | Registrira MVC, DbContext, Identity, repozitorije, logging, health endpoint i rute. | "Program.cs je centralna konfiguracija aplikacije." |
| Baza i relacije | `InventoryManagement.DAL/InventoryManagementDbContext.cs` | Definira DbSetove, relacije, delete pravila, decimalne tipove i seed podatke. | "EF Core mapira domenske modele u SQL Server tablice." |
| Domenski modeli | `InventoryManagement.Domain/Models/*` | Predstavljaju poslovne entitete: Product, Supplier, Warehouse, Order itd. | "Modeli su struktura podataka aplikacije." |
| MVC UI | `InventoryManagement/Controllers/*Controller.cs` i `Views/*` | Prikazuje stranice, forme, detalje i tablice. | "MVC controller priprema podatke, a Razor view ih prikazuje." |
| API | `InventoryManagement/Controllers/Api/*ApiController.cs` | Vraca JSON i omogucuje CRUD kroz HTTP metode. | "API je odvojen od UI-ja i koristi DTO objekte." |
| DTO mapiranje | `InventoryManagement/DTOs/ApiDtos.cs`, `MappingExtensions.cs` | Razdvaja interne modele od JSON odgovora i zahtjeva. | "DTO ogranicava sto API prima i vraca." |
| Autentikacija | `AccountController.cs`, `IdentitySeedData.cs`, `AppUser.cs` | Lokalna prijava, Google login ako je konfiguriran, uloge Admin/Manager. | "Identity upravlja korisnicima i ulogama." |
| Testovi | `InventoryManagement.Tests/*` | Provjeravaju aplikaciju kroz testni web server i InMemory bazu. | "Testovi pokrecu stvarne HTTP zahtjeve prema aplikaciji." |
| Deployment | `DEPLOY.md`, `Dockerfile` | Opisuje produkcijske postavke, tajne vrijednosti i Docker pokretanje. | "Konfiguracija se postavlja iz environment varijabli, ne kroz kod." |

## Glavni tokovi koje treba znati objasniti

### 1. Prikaz kataloga proizvoda

1. Korisnik otvara `/catalog`.
2. `ProductController.Index()` poziva `ProductEfRepository.GetAll()`.
3. Repozitorij kroz EF Core cita proizvode, kategorije i dobavljace.
4. View `Views/Product/Index.cshtml` prikazuje tablicu.

Bitno za obranu: javni pregled je dozvoljen preko `[AllowAnonymous]`, a izmjene su ogranicene na korisnike s ulogom `Admin`.

### 2. Kreiranje ili uredjivanje proizvoda

1. Admin otvara formu za create ili edit.
2. Controller prima `ProductFormModel`.
3. Ako `ModelState` nije valjan, vraca se ista forma s greskama.
4. Ako je model valjan, podaci se mapiraju u `Product`.
5. Repozitorij sprema promjene preko EF Corea.

Bitno za obranu: forma ne prima direktno cijeli EF model, nego koristi poseban view model. Time je jasnije sto korisnik smije unijeti.

### 3. API CRUD tok

1. Klijent salje zahtjev na npr. `/api/products`.
2. API controller koristi `ProductRequestDto` za ulaz.
3. Validacija se radi atributima kao `[Required]`, `[Range]`, `[StringLength]`.
4. Controller provjerava postoje li povezani zapisi, npr. kategorija i dobavljac.
5. Odgovor se vraca kao `ProductDto`.

Bitno za obranu: API ne vraca cijele EF entitete, nego DTO. To smanjuje rizik od slucajnog izlaganja nepotrebnih polja i kruznih referenci.

### 4. Globalna pretraga

1. Korisnik otvara `/search?q=Business`.
2. `GlobalSearchController` poziva vise repozitorija.
3. Svaki modul vraca ogranicen broj rezultata.
4. Rezultati se mapiraju u `GlobalSearchResultViewModel` i grupiraju po modulu.

Bitno za obranu: pretraga je read-only i javna, a svaki rezultat ima naslov, opis i link na detalje.

### 5. AI savjet za proizvod

1. Korisnik na detaljima proizvoda salje POST na `/catalog/{id}/ai-advice`.
2. `ProductController.AiAdvice()` ucitava proizvod.
3. `ProductAiAdvisor` provjerava postoji li `OpenAI:ApiKey`.
4. Ako kljuc nije konfiguriran, vraca sigurnu poruku umjesto greske.
5. Ako je kljuc konfiguriran, servis salje sazet prompt prema OpenAI Responses API-ju.

Bitno za obranu: aplikacija ne zahtijeva API kljuc za normalan rad. Vanjski servis je opcionalan i ima fallback.

### 6. Logiranje zahtjeva

1. `Program.cs` registrira custom file logger.
2. Middleware mjeri trajanje svakog HTTP zahtjeva.
3. Uspjesni i neuspjesni zahtjevi zapisuju se u log.
4. `FileLoggerTests.cs` provjerava da logger zapisuje formatiranu poruku.

Bitno za obranu: logiranje pomaze pri dijagnostici i ne smije zapisivati tajne vrijednosti.

### 7. Deployment i konfiguracija

1. Connection string, Google OAuth i OpenAI kljuc dolaze iz konfiguracije ili environment varijabli.
2. Produkcijska aplikacija koristi SQL Server.
3. Testno okruzenje koristi InMemory bazu.
4. `/health` endpoint brzo potvrdjuje da je aplikacija pokrenuta.

Bitno za obranu: tajne vrijednosti nisu hardkodirane u repozitoriju.

## Datoteke koje vrijedi otvoriti na obrani

- `InventoryManagement/Program.cs` - pokazuje kako je aplikacija konfigurirana.
- `InventoryManagement.DAL/InventoryManagementDbContext.cs` - pokazuje tablice, relacije i seed podatke.
- `InventoryManagement/Controllers/ProductController.cs` - dobar primjer MVC toka.
- `InventoryManagement/Controllers/Api/ProductApiController.cs` - dobar primjer API toka.
- `InventoryManagement/DTOs/ApiDtos.cs` - pokazuje validaciju ulaznih DTO objekata.
- `InventoryManagement/Services/ProductAiAdvisor.cs` - pokazuje opcionalnu integraciju s vanjskim servisom i fallback.
- `InventoryManagement.Tests/ApiCrudIntegrationTests.cs` - pokazuje dokaz da API radi.
- `InventoryManagement.Tests/CustomWebApplicationFactory.cs` - pokazuje kako testovi pokrecu aplikaciju u testnom okruzenju.
- `DEPLOY.md` - pokazuje da je projekt pripremljen za pokretanje izvan lokalnog racunala.

## Pitanja koja se mogu pojaviti na obrani

### Zasto postoji i MVC controller i API controller?

MVC controller vraca HTML stranice za korisnika u browseru. API controller vraca JSON za druge klijente, testove ili buduce integracije. Time su UI i API jasno razdvojeni.

### Zasto se koriste DTO objekti?

DTO objekti odvajaju vanjski API ugovor od internih EF modela. Na taj nacin kontroliramo sto korisnik smije poslati i sto aplikacija smije vratiti.

### Kako je rijesena sigurnost?

Projekt koristi ASP.NET Core Identity, lokalnu prijavu i opcionalni Google login. Javni read-only prikazi imaju `[AllowAnonymous]`, a create, edit i delete akcije su uglavnom ogranicene na `Admin` ulogu. API endpointi takodjer koriste role-based autorizaciju.

### Kako se sprjecava brisanje zapisa koji se koriste drugdje?

Za proizvode se prije brisanja provjerava postoje li povezane stavke narudzbi ili stanje zaliha. Ako postoje, brisanje se odbija. U DbContextu su definirana i delete pravila poput `Restrict`, `Cascade` i `SetNull`.

### Kako rade testovi bez prave baze?

`CustomWebApplicationFactory` pokrece aplikaciju u `Testing` okruzenju i zamjenjuje SQL Server konfiguraciju InMemory bazom. Tako integracijski testovi salju stvarne HTTP zahtjeve, ali ne ovise o lokalnom SQL Serveru.

### Sto se dogadja ako OpenAI API kljuc nije postavljen?

`ProductAiAdvisor` vraca poruku da AI asistent nije konfiguriran. Aplikacija se ne rusi i korisnik i dalje moze koristiti ostale funkcionalnosti.

### Zasto su connection string i API kljucevi u konfiguraciji?

Zato sto su to tajne ili okolinski ovisne vrijednosti. Ne smiju biti hardkodirane u kodu ni spremljene u repozitorij.

### Sto je najvazniji dokaz kvalitete projekta?

Najvazniji dokaz su integracijski testovi za API CRUD, test globalne pretrage, test file loggera, dokumentirani Playwright scenarij i deployment dokumentacija.

## Kratki demo scenarij za obranu

1. Pokreni aplikaciju i otvori dashboard.
2. Otvori Products i pokazi seedane proizvode.
3. Pretrazi `Business` i pokazi AJAX filtriranje tablice.
4. Otvori detalje proizvoda i objasni relacije s kategorijom, dobavljacem, skladistem i narudzbama.
5. Otvori globalnu pretragu `/search?q=Business`.
6. Pokazi jedan API endpoint, npr. `/api/products?q=laptop`.
7. Otvori testove i pokazi da se CRUD provjerava integracijski.
8. Pokazi `DEPLOY.md` i objasni environment varijable.

## Naredbe za provjeru

Iz foldera `InventoryManagement`:

```powershell
dotnet build InventoryManagement.slnx
dotnet test InventoryManagement.slnx
```

Za lokalno pokretanje aplikacije obicno se koristi:

```powershell
dotnet run --project InventoryManagement/InventoryManagement.csproj
```

Ako se koristi produkcijsko okruzenje, treba postaviti connection string i ostale potrebne environment varijable prema `DEPLOY.md`.

## Sto ne treba tvrditi ako nije pokazano

- Ne tvrditi da je Google login uvijek aktivan; aktivan je samo ako su postavljeni `Authentication:Google:ClientId` i `Authentication:Google:ClientSecret`.
- Ne tvrditi da je OpenAI savjet uvijek dostupan; dostupan je samo uz `OpenAI:ApiKey`.
- Ne tvrditi da Playwright test automatski postoji u projektu; trenutno je dokumentiran scenarij i primjer implementacije.
- Ne tvrditi da produkcijska baza sama migrira uvijek; automatske migracije ovise o postavci `Database:ApplyMigrations=true`.

## Zavrsna recenica za obranu

"Projekt sam organizirao kao ASP.NET Core MVC aplikaciju s odvojenim UI i API slojem, EF Core modelom podataka, Identity autentikacijom, konfiguracijom za deployment i integracijskim testovima koji provjeravaju glavne tokove. Najvaznije mi je da se svaka funkcionalnost moze povezati s konkretnim controllerom, modelom, konfiguracijom i testom."
