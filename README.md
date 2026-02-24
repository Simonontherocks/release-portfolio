🎬 Film Collection Manager
📌 Over dit project

Film Collection Manager is een individueel ontwikkeld eindwerk in C#.
De webapplicatie laat toe een persoonlijke filmcollectie te beheren via een gestructureerde, gelaagde architectuur.

Het project demonstreert niet enkel MVC, maar ook een duidelijke scheiding van verantwoordelijkheden via een afzonderlijke Data Access Layer en Business Layer, inclusief unit testing.

🚀 Functionaliteiten
🎥 Filmbeheer (CRUD)

- Film toevoegen
- Film verwijderen
- Film bewerken
- Overzicht van volledige collectie

------------------------------------------------

👁 Statusbeheer

Markeren als “gezien” of “niet gezien”
- Overzichtslijsten:
 - Alle films
 - Gezien
 - Niet gezien

🔎 Zoekfunctionaliteit

Zoeken en filteren op:

- Acteur
- Regisseur
- Jaar

Resultaten worden gestructureerd weergegeven in aparte overzichtslijsten.

------------------------------------------------

🏗 Architectuur

De applicatie werd ontwikkeld volgens een meerlagige architectuur:

1️⃣ ModelLayer

Bevat de domeinmodellen en entiteiten.

2️⃣ Data Access Layer (DAL)
Verantwoordelijk voor:
- Database-interactie
- Query’s
- Data persistence
- Scheiding tussen businesslogica en opslag

3️⃣ Business Layer (BL)

Bevat:
- Validaties
- Business rules
- Verwerkingslogica
- Orchestratie tussen DAL en Presentation

4️⃣ Presentation Layer

Bevat de MVC-structuur:
- Controllers
- Views
- ViewModels

5️⃣ MainProgram (Test Entry)

Een aparte entry point om businesslogica rechtstreeks te testen buiten de UI-context.
Daarnaast werden formele unit tests geïmplementeerd voor kritieke onderdelen van de businesslogica.

------------------------------------------------

🛠 Gebruikte Technologieën

- C#
- ASP.NET MVC
- Web API
- HTML5
- CSS3
- JavaScript
- Microsoft SQL Server
- SQL Server Management Studio 18
- Unit Testing Framework

------------------------------------------------

Het project bevat:

- Unit tests voor businesslogica
- Gescheiden testbare componenten
- Een aparte test entry via MainProgram

Dit verhoogt de onderhoudbaarheid en betrouwbaarheid van de applicatie.

------------------------------------------------

🗄 Database

De applicatie maakt gebruik van een lokale Microsoft SQL Server database.
Films worden persistent opgeslagen via de Data Access Layer.
Een geldige connection string is vereist in de configuratie.

------------------------------------------------

⚙ Installatie (Lokaal gebruik)

1. Clone de repository
2. Open de solution in Visual Studio 2022
3. Configureer de connection string naar je lokale SQL Server instantie
4. Voer eventuele migraties of SQL-scripts uit
5. Start de applicatie

------------------------------------------------

🎯 Doel van het project

Dit project werd ontwikkeld om volgende competenties aan te tonen:

- Ontwikkeling van een volledige webapplicatie
- Implementatie van een gelaagde architectuur
- Scheiding van verantwoordelijkheden
- CRUD-operaties en filtering
- Database-integratie
- Testbaarheid van businesslogica
- Zelfstandig projectbeheer

------------------------------------------------

⚖ Licentie & Gebruik

© Simon Gryspeert

Dit project is auteursrechtelijk beschermd.
Het mag niet worden gekopieerd, gedistribueerd of gebruikt zonder uitdrukkelijke schriftelijke toestemming van de auteur.
