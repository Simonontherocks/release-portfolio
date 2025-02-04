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
    public class AppDBContext : DbContext
    {
        #region Property        

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<MovieDirector> MovieDirectors { get; set; }
        
        // public DbSet<Person> Persons { get; set; }

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


    }

}
