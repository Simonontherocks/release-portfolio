using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineVault.ModelLayer.ModelMovie;

namespace CineVault.DataAccessLayer.Repositories
{
    /*
     * Deze interface zal dienen als blauwprint over hoe de werkelijke repository er uit zal moeten zien.
     * Hier worden enkel de nodige methodes geschreven, die verplicht geïmplementeerd moeten worden, door de klasse die gebruik zal maken van de interface.
     */
    public interface IMovieRepository
    {
        // Het doel van IEnumerable is om over een lijst te itereren en de benodigde gegevens op te halen.

        // ToDo: Toegevoegd aan de interdface. In de klasse zelf nog controleren of deze methode werkt.
        public bool CheckIfMovieExists(Movie movie); //(string title, string releaseDate);

        // Hieronder staan de signaturen van de methodes om een film toe te voegen of te verwijderen.
        public void AddMovieByMovie(Movie movie); // Toevoegen van een film.
        public void RemoveMovieByMovie(Movie movie); // Verwijderen van een film.

        // Hieronder staan de signaturen van de methodes om lijsten met films te filteren op basis van wat de gebruiker in bezit heeft en of hij deze al dan niet heeft gezien.
        public IEnumerable<Movie> ShowAllMoviesAUserContains(); // Hier worden alle films getoont die de gebruiker in bezit heeft.
        public IEnumerable<Movie> ShowAllMoviesThatHaveBeenSeen(); // Hier worden alle films getoont dat de gebruiker heeft gezien.
        public IEnumerable<Movie> ShowAllMoviesThatHaveNotBeenSeen(); // Hier worden alle films getoont dat de gebruiker nog niet heeft gezien.

        // Hieronder staan de signaturen van de methodes om lijsten met films te filteren op basis van acteur, regisseur of jaar.
        public IEnumerable<Actor> ShowAllActorsFromMovie(Movie movie); // Hier worden alle acteurs getoont die meespelen in de desbetreffende film.
        public IEnumerable<Director> ShowDirectorFromMovie(Movie movie); // Hier wordt de regisseur of meerdere regisseurs getoond die de desbetreffende film hebben geregisseerd.
        public string ShowYearFromMovie(Movie movie); // Hier wordt het jaar van verschijning getoond van de desbetreffende film.

        // This filters on a model and returns a list.
        public IEnumerable<Movie> ShowMoviesFromTheSameActor(Actor actor); // Hier worden alle films getoont waar een bepaalde acteur in meespeelt.
        public IEnumerable<Movie> ShowMoviesFromTheSameDirector(Director director); // Hier worden alle films getoont die door een bepaalde acteur zijn geregisseerd.
        public IEnumerable<Movie> ShowAllMoviesFromTheSameYear(string strYear); // Hier worden alle films getoont die in een bepaald jaar zijn verschenen.

    }
}
