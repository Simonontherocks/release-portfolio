using CineVault.ModelLayer.ModelAbstractClass;
using CineVault.ModelLayer.ModelMovie;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.DataAccessLayer
{
    // Add-Migration InitialCreate -Project Quest.Data -StartupProject Quest.UI
    // Update-Database -Project Quest.Data -StartupProject Quest.UI
    public class AppDBContext : DbContext
    {
        #region Property        

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<MovieDirector> MovieDirectors { get; set; }
        
        #endregion


        #region Constructor

        public AppDBContext(DbContextOptions<AppDBContext> options) //, IConfiguration configuration)
            : base(options)
        {
            
        }

        #endregion

        #region OnConfiguring

        // Deze toevoeging zorgt ervoor dat je AppDBContext weet waar de migraties zich bevinden.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=SIMON\\SQLEXPRESS;Database=CineVault; Trusted_Connection=true; TrustServerCertificate=True");
        }

        #endregion

        #region Toevoegen van mockdata

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Voeg mock-acteurs toe
            modelBuilder.Entity<Actor>().HasData(
                new Actor { Id = 1, Name = "Leonardo DiCaprio" },
                new Actor { Id = 2, Name = "Joseph Gordon-Levitt" },
                new Actor { Id = 3, Name = "Elliot Page" },
                new Actor { Id = 4, Name = "John Travolta" },
                new Actor { Id = 5, Name = "Samuel L. Jackson" },
                new Actor { Id = 6, Name = "Matthew McConaughey" },
                new Actor { Id = 7, Name = "Anne Hathaway" },
                new Actor { Id = 8, Name = "Kate Winslet" },
                new Actor { Id = 9, Name = "Quentin Tarantino" }
            );

            // Voeg mock-regisseurs toe
            modelBuilder.Entity<Director>().HasData(
                new Director { Id = 1, Name = "Christopher Nolan" },
                new Director { Id = 2, Name = "Quentin Tarantino" },
                new Director { Id = 3, Name = "James Cameron" },
                new Director { Id = 4, Name = "Robert Rodriguez" },
                new Director { Id = 5, Name = "George Lucas" }
            );

            // Voeg mock-films toe
            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Title = "Inception", Seen = true, Score = 9.0, Year = "2010" },
                new Movie { Id = 2, Title = "Pulp Fiction", Seen = false, Score = 9, Year = "1994" },
                new Movie { Id = 3, Title = "Interstellar", Seen = true, Score = 8.5, Year = "2014" },
                new Movie { Id = 4, Title = "Titanic", Seen = true, Score = 8.0, Year = "1997" },
                new Movie { Id = 5, Title = "Sin City", Seen = false, Score = 7.0, Year = "2005" },
                new Movie { Id = 6, Title = "Star Wars", Seen = true, Score = 7.5, Year = "2005"}
            );

            // Relaties toevoegen in tussen-tabellen
            modelBuilder.Entity<MovieActor>().HasData(
                // Inception (2010)
                new MovieActor { Id = 1, MovieId = 1, ActorId = 1 }, // Leonardo DiCaprio
                new MovieActor { Id = 2, MovieId = 1, ActorId = 2 }, // Joseph Gordon-Levitt
                new MovieActor { Id = 3, MovieId = 1, ActorId = 3 }, // Elliot Page

                // Pulp Fiction (1994)
                new MovieActor { Id = 4, MovieId = 2, ActorId = 4 }, // John Travolta
                new MovieActor { Id = 5, MovieId = 2, ActorId = 5 }, // Samuel L. Jackson
                new MovieActor { Id = 6, MovieId = 2, ActorId = 9 }, // Quentin Tarantino (Regisseur & Acteur)

                // Interstellar (2014)
                new MovieActor { Id = 7, MovieId = 3, ActorId = 6 }, // Matthew McConaughey
                new MovieActor { Id = 8, MovieId = 3, ActorId = 7 }, // Anne Hathaway

                // Titanic (1997)
                new MovieActor { Id = 9, MovieId = 4, ActorId = 1 }, // Leonardo DiCaprio
                new MovieActor { Id = 10, MovieId = 4, ActorId = 8 }, // Kate Winslet

                // Sin City (2005)
                new MovieActor { Id = 11, MovieId = 5, ActorId = 9 }, // Quentin Tarantino (Regisseur & Acteur)

                // Star Wars (2005)
                new MovieActor { Id = 12, MovieId = 6, ActorId = 5 } // Samuel L. Jackson
            );

            modelBuilder.Entity<MovieDirector>().HasData(
                new MovieDirector { Id = 1, MovieId = 1, DirectorId = 1 }, // Inception - Christopher Nolan
                new MovieDirector { Id = 2, MovieId = 2, DirectorId = 2 }, // Pulp Fiction - Quentin Tarantino
                new MovieDirector { Id = 3, MovieId = 3, DirectorId = 1 }, // Interstellar - Christopher Nolan
                new MovieDirector { Id = 4, MovieId = 4, DirectorId = 3 }, // Titanic - James Cameron
                new MovieDirector { Id = 5, MovieId = 5, DirectorId = 2 }, // Sin City - Quentin Tarantino
                new MovieDirector { Id = 6, MovieId = 5, DirectorId = 4 }, // Sin City - Robert Rodriguez
                new MovieDirector { Id = 7, MovieId = 6, DirectorId = 5 } // Star Wars - George Lucas
            );

        }


        #endregion

    }

}
