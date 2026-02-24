using CineVault.BusinessLogic.Service; // Importeert de services voor de business logic
using CineVault.DataAccessLayer; // Importeert de data access layer
using CineVault.DataAccessLayer.Context; // Importeert de databasecontext
using CineVault.DataAccessLayer.Repositories; // Importeert de repository-klassen
using Microsoft.EntityFrameworkCore; // Importeert Entity Framework Core voor databasebeheer

namespace CineVault.PresentationLayor.Website
{
    /// <summary>
    /// Dit is de entry-point van de CineVault-webapplicatie. 
    /// Het initialiseert services, configureert de databaseverbinding,
    /// stelt dependency injection in, en definieert de routing voor de applicatie.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); // Maakt een nieuwe webapplicatie en configureert deze

            // Voeg controllers en views toe aan de services-container
            builder.Services.AddControllersWithViews(); // Nodig voor MVC-architectuur

            // Laad de databaseconfiguratie uit appsettings.json
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Zet de basisdirectory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // Laadt de configuratie van de databaseverbinding

            // Dependency Injection Configuratie:
            //
            // - `AddDbContext<AppDBContext>`: 
            //   - Wordt gebruikt voor database-interacties via Entity Framework Core.
            //   - Scoped: Elke HTTP-request krijgt een eigen instantie van `AppDBContext` om dataconflicten te vermijden.
            //
            // - `AddScoped<IMovieRepository, MovieRepository>` / `AddScoped<MovieService>`:
            //   - Scoped: Elke HTTP-request krijgt een aparte instantie.
            //   - Dit is nuttig voor services die databasebewerkingen uitvoeren, zodat binnen dezelfde request één consistente instantie wordt gebruikt.
            //
            // - `AddSingleton<ApiService>`:
            //   - Singleton: Blijft actief gedurende de hele applicatie.
            //   - Dit is geschikt voor een externe API-service die geen afhankelijkheid heeft van de database en hergebruikbare data kan bewaren.
            //
            // - Waarom geen `AddTransient` gebruikt?
            //   - `AddTransient` creëert bij elke aanroep een nieuwe instantie.
            //   - Dit is niet optimaal voor database-interacties, omdat het kan leiden tot meerdere instanties van dezelfde context binnen één request, wat dataconflicten en prestatieproblemen kan veroorzaken.
            //   - `Scoped` voorkomt dat problemen door consistentie binnen een HTTP-request te garanderen.

            IConfigurationRoot config = configBuilder.Build(); // Bouwt de configuratie
            builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection"))); // Stelt de databasecontext in met SQL Server

            // Configuratie van dependency injection voor repositories en services
            builder.Services.AddScoped<IMovieRepository, MovieRepository>(); // Koppelt `IMovieRepository` interface aan `MovieRepository` klasse
            builder.Services.AddScoped<MovieService>(); // Zorgt ervoor dat `MovieService` per HTTP-request een nieuwe instantie krijgt
            builder.Services.AddSingleton<ApiService>(); // `ApiService` blijft dezelfde instantie zolang de applicatie draait
            builder.Services.AddScoped<ActorService>(); // Actor-service krijgt een nieuwe instantie per HTTP-request
            builder.Services.AddScoped<IActorRepository, ActorRepository>(); // Actor-repository instellen via dependency injection
            builder.Services.AddScoped<IDirectorRepository, DirectorRepository>(); // Regisseur-repository instellen
            builder.Services.AddScoped<DirectorService>(); // Regisseur-service instellen

            var app = builder.Build(); // Bouwt de webapplicatie met de geconfigureerde services

            // Configureer de HTTP request pipeline
            if (!app.Environment.IsDevelopment()) // Controleert of de app niet in ontwikkelingsmodus is
            {
                app.UseExceptionHandler("/Home/Error"); // Gebruikt een standaard foutpagina als er een error optreedt
            }
            app.UseStaticFiles(); // Hiermee kan de app statische bestanden (CSS, JS, afbeeldingen) serveren

            app.UseRouting(); // Maakt het mogelijk om routes te definiëren

            app.UseAuthorization(); // Handelt autorisatie af voor beveiligde routes

            app.MapControllerRoute( // Definieert een standaard route voor de controllers
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"); // Stelt de standaard controller en actie in

            app.Run(); // Start de webapplicatie
        }
    }
}
