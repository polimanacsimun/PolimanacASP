# MCP / Agentic IDE pristup

Ovaj dokument opisuje kako je u projektu koristen agentic IDE pristup: AI asistent radi unutar istog repozitorija, cita postojece datoteke, predlaze izmjene, mijenja kod i zatim provjerava rezultat kroz build ili testove.

## Cilj pristupa

Cilj nije bio samo generirati kod, nego koristiti AI kao kontroliranog pomocnika u razvoju:

- prvo razumjeti postojecu strukturu projekta
- raditi male, provjerljive izmjene
- oslanjati se na postojece obrasce u controllerima, servisima i testovima
- nakon izmjene provjeriti ponasanje aplikacije testovima ili rucnim scenarijem
- dokumentirati sto je napravljeno i gdje se moze provjeriti

## Kako je pristup primijenjen

Agentic IDE pristup koristen je za rad na vise dijelova aplikacije:

- prosirenje API CRUD pokrivenosti kroz integracijske testove
- dodavanje sigurnih fallback scenarija za AI savjete proizvoda
- dokumentiranje Playwright browser scenarija za javni UI i AJAX pretragu
- pripremu deployment dokumentacije i Docker konfiguracije
- provjeru konfiguracije bez upisivanja tajnih vrijednosti u repozitorij

AI alat nije tretiran kao autoritet. Svaka znacajnija izmjena treba biti citljiva u kodu i provjerljiva kroz testove, dokumentirani browser scenarij ili konfiguracijski artefakt.

## MCP ideja u kontekstu projekta

MCP pristup ovdje znaci da agent ne radi samo nad izoliranim promptom, nego koristi kontekst projekta i dostupne alate:

- filesystem za citanje i izmjenu datoteka u repozitoriju
- terminal za pokretanje builda i testova
- git stanje za razlikovanje novih izmjena od ranije postojecih promjena
- projektne datoteke kao izvor istine umjesto slobodnog izmisljanja arhitekture

Takav rad smanjuje rizik da AI generira kod koji izgleda ispravno, ali ne odgovara stvarnoj aplikaciji.

## Pravila rada s AI agentom

U projektu su korisna sljedeca pravila:

1. Prvo pregledati postojece controllere, servise, view modele i testove.
2. Ne uvoditi novu arhitekturu ako postojeci obrazac vec rjesava problem.
3. Ne spremati API kljuceve, lozinke ni osobne podatke u repozitorij.
4. Za vanjske servise koristiti konfiguraciju i fallback ponasanje.
5. Za API funkcionalnosti preferirati integracijske testove umjesto testova koji samo mockaju vlastiti kod.
6. Za UI tokove imati barem jedan jasan browser scenarij koji se moze rucno ili automatizirano proci.

## Dokazi u repozitoriju

Relevantni artefakti:

- `InventoryManagement.Tests/ApiCrudIntegrationTests.cs` - integracijski CRUD testovi za API endpointe
- `InventoryManagement.Tests/ProductAiAdviceIntegrationTests.cs` - provjera AI fallback ponasanja bez konfiguriranog API kljuca
- `InventoryManagement.Tests/PlaywrightScenario.md` - opis browser smoke scenarija za javni UI
- `InventoryManagement.Tests/README.md` - kratke upute za pokretanje testova
- `DEPLOY.md` - deployment i konfiguracijske upute
- `ZAVRSNA_PRIPREMA_OBRANA.md` - sazetak arhitekture, obrambena pitanja i demo scenarij
- `Dockerfile` i `.dockerignore` - priprema aplikacije za pokretanje u kontejneru

## Ogranicenja

Agentic IDE pristup ne zamjenjuje razumijevanje koda. Posebno treba rucno pregledati:

- autorizacijska pravila
- validaciju ulaznih podataka
- brisanje i izmjenu zapisa
- konfiguraciju vanjskih servisa
- dijelove koji ovise o produkcijskom okruzenju

AI moze ubrzati razvoj, ali zavrsna odgovornost ostaje na developeru: kod mora biti pregledan, testovi moraju prolaziti, a konfiguracija mora biti sigurna.
