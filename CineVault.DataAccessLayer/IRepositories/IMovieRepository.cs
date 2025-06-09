using CineVault.ModelLayer.ModelMovie;

namespace CineVault.DataAccessLayer.Repositories
{
    /// <summary>
    /// - IMovieRepository - Interface voor de Movie Repository
    /// - Definieert de contractuele methodes die door `MovieRepository` moeten worden geïmplementeerd.  
    /// - Waarom een interface?  
    ///   - Zorgt ervoor dat de repository testbaar en flexibel blijft.  
    ///   - Maakt het mogelijk om verschillende implementaties van de repository te gebruiken.  
    /// - Functionaliteiten:
    ///   - Ophalen, toevoegen, bijwerken en verwijderen van films.  
    ///   - Filteren van films op status (gezien/niet gezien).  
    ///   - Opvragen van acteurs en regisseurs per film.  
    /// </summary>
    
    public interface IMovieRepository
    {
        // Het doel van IEnumerable is om over een lijst te itereren en de benodigde gegevens op te halen.

        #region Checking existence

        public bool CheckIfMovieExists(Movie movie); // Controleert of een film al bestaat in de database op basis van de TMDB ID

        #endregion

        #region Adding or removing movies

        // Hieronder staan de signaturen van de methodes om een film toe te voegen of te verwijderen.
        public void AddMovieByMovie(Movie movie); // Methode om een film toe te voegen aan de database
        public void RemoveMovieByMovie(Movie movie); // Methode om een film uit de database te verwijderen

        #endregion

        #region Retrieve movies by status

        // Hieronder staan de signaturen van de methodes om lijsten met films te filteren op basis van wat de gebruiker in bezit heeft en of hij deze al dan niet heeft gezien.
        public IEnumerable<Movie> ShowAllMoviesAUserContains(); // Methode om alle films op te halen die een gebruiker in bezit heeft
        public IEnumerable<Movie> ShowAllMoviesThatHaveBeenSeen(); // Methode om alle films op te halen die de gebruiker heeft gezien
        public IEnumerable<Movie> ShowAllMoviesThatHaveNotBeenSeen(); // Methode om alle films op te halen die de gebruiker nog niet heeft gezien

        #endregion

        #region Filter by movie data

        // Hieronder staan de signaturen van de methodes om lijsten met films te filteren op basis van acteur, regisseur of jaar.
        public IEnumerable<Actor> ShowAllActorsFromMovie(Movie movie); // Methode om alle acteurs op te halen die in een specifieke film spelen
        public IEnumerable<Director> ShowDirectorFromMovie(Movie movie); // Methode om alle regisseurs op te halen die een specifieke film hebben geregisseerd
        public string ShowYearFromMovie(Movie movie); // Methode om het jaar van verschijning van een specifieke film op te halen

        #endregion

        #region Filter by model

        // This filters on a model and returns a list.
        public IEnumerable<Movie> ShowMoviesFromTheSameActor(Actor actor); // Methode om alle films op te halen waarin een specifieke acteur speelt
        public IEnumerable<Movie> ShowMoviesFromTheSameDirector(Director director); // Methode om alle films op te halen die door een specifieke regisseur zijn geregisseerd
        public IEnumerable<Movie> ShowAllMoviesFromTheSameYear(string strYear); // Methode om alle films op te halen die in een bepaald jaar zijn verschenen

        #endregion

        #region Checking by Id

        // Deze methode zal dienen om een film te kunnen opzoeken via een id
        public Movie GetById(int id); // Methode om een specifieke film op te halen via een unieke ID

        #endregion

    }
}
