# 🎬 Film Collection Manager

[🇳🇱 Nederlands](#-nederlands) | [🇬🇧 English](#-english)

---

# 🇳🇱 Nederlands

## 📌 Over dit project

Film Collection Manager is een individueel ontwikkeld eindwerk in C#.  
De webapplicatie laat toe een persoonlijke filmcollectie te beheren via een gestructureerde, gelaagde architectuur.

Het project demonstreert niet enkel MVC, maar ook een duidelijke scheiding van verantwoordelijkheden via een afzonderlijke Data Access Layer en Business Layer, inclusief unit testing.

---

## 🚀 Functionaliteiten

### 🎥 Filmbeheer (CRUD)

- Film toevoegen  
- Film verwijderen  
- Film bewerken  
- Overzicht van volledige collectie  

### 👁 Statusbeheer

- Markeren als “gezien” of “niet gezien”  

Overzichtslijsten:
- Alle films  
- Gezien  
- Niet gezien  

### 🔎 Zoekfunctionaliteit

Zoeken en filteren op:
- Acteur  
- Regisseur  
- Jaar  

Resultaten worden gestructureerd weergegeven in aparte overzichtslijsten.

---

## 🏗 Architectuur

De applicatie werd ontwikkeld volgens een meerlagige architectuur:

### ModelLayer
Bevat de domeinmodellen en entiteiten.

### Data Access Layer (DAL)
- Database-interactie  
- Query’s  
- Data persistence  
- Scheiding tussen businesslogica en opslag  

### Business Layer (BL)
- Validaties  
- Business rules  
- Verwerkingslogica  
- Orchestratie tussen DAL en Presentation  

### Presentation Layer
Bevat de MVC-structuur:
- Controllers  
- Views  
- ViewModels  

### MainProgram (Test Entry)
Een aparte entry point om businesslogica rechtstreeks te testen buiten de UI-context.  
Daarnaast werden formele unit tests geïmplementeerd.

---

## 🛠 Technologieën

- C#  
- ASP.NET MVC  
- Web API  
- HTML5  
- CSS3  
- JavaScript  
- Microsoft SQL Server  
- SQL Server Management Studio 18  
- Unit Testing Framework  

---

## 🗄 Database

De applicatie maakt gebruik van een lokale Microsoft SQL Server database.
In de map Database bevindt zich een SQL-script waarmee de benodigde database en tabellen kunnen worden aangemaakt via SQL Server Management Studio.
Een geldige connection string is vereist in appsettings.json.
Pas deze aan naar jouw lokale SQL Server instantie indien nodig.

Standaardvoorbeeld:

Server=.\SQLEXPRESS;Database=CineVault;Trusted_Connection=True;TrustServerCertificate=True;

---

## ⚙ Installatie (Lokaal gebruik)

1. Clone de repository  
2. Open de solution in Visual Studio 2022  
3. Configureer de connection string  
4. Voer eventuele migraties of SQL-scripts uit  
5. Start de applicatie  

---

## ⚖ Licentie & Gebruik

© Simon Gryspeert  

Dit project is auteursrechtelijk beschermd.  
Het mag niet worden gekopieerd, gedistribueerd of gebruikt zonder uitdrukkelijke schriftelijke toestemming van de auteur.

---

# 🇬🇧 English

## 📌 About This Project

Film Collection Manager is an individually developed final-year project written in C#.  
The web application allows users to manage a personal movie collection using a structured, layered architecture.

This project demonstrates not only MVC, but also a clear separation of concerns through a dedicated Data Access Layer and Business Layer, including unit testing.

---

## 🚀 Features

### 🎥 Movie Management (CRUD)

- Add a movie  
- Delete a movie  
- Edit a movie  
- View the complete collection  

### 👁 Watch Status Management

- Mark movies as “Watched” or “Not Watched”  

Overview lists:
- All movies  
- Watched movies  
- Unwatched movies  

### 🔎 Search Functionality

Search and filter by:
- Actor  
- Director  
- Release year  

Results are displayed in structured overview lists.

---

## 🏗 Architecture

The application was developed using a layered architecture:

### Model Layer
Contains domain models and entities.

### Data Access Layer (DAL)
- Database interaction  
- Query execution  
- Data persistence  
- Separation between business logic and storage  

### Business Layer (BL)
- Validation logic  
- Business rules  
- Processing logic  
- Coordination between DAL and Presentation layer  

### Presentation Layer
Implements the MVC structure:
- Controllers  
- Views  
- ViewModels  

### MainProgram (Test Entry)
A separate entry point to test business logic outside the UI context.  
Formal unit tests were implemented for critical components.

---

## 🛠 Technologies Used

- C#  
- ASP.NET MVC  
- Web API  
- HTML5  
- CSS3  
- JavaScript  
- Microsoft SQL Server  
- SQL Server Management Studio 18  
- Unit Testing Framework  

---

## 🗄 Database

🗄 Database

The application uses a local Microsoft SQL Server database.
The Database folder contains a SQL script to create the required database and tables using SQL Server Management Studio.
A valid connection string must be configured in appsettings.json.
Adjust it to match your local SQL Server instance if necessary.

Default example:

Server=.\SQLEXPRESS;Database=CineVault;Trusted_Connection=True;TrustServerCertificate=True;

---

## ⚖ License & Usage

© Simon Gryspeert  

This project is protected by copyright.  
It may not be copied, distributed, or used without explicit written permission from the author.
