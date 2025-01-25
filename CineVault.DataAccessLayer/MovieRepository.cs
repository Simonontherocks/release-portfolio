using CineVault.ModelLayer.ModelMovie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.DataAccessLayer
{
    public class MovieRepository : IMovieRepository
    {
        #region Field

        //private readonly List<Movie> _lstMovies = new List<Movie>();
        //private readonly List<Actor> _lstActors = new List<Actor>();
        //private readonly List<Director> _lstDirectors = new List<Director>();
        private AppDBContext _dbContext;

        #endregion

        #region Constructor

        public MovieRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Adding or removing movies

        //public void AddMovieByMovie(Movie movie)
        //{
        //    if (movie.IMDBEntry != null && string.IsNullOrEmpty(movie.CoverUrl))
        //    {
        //        movie.CoverUrl = movie.IMDBEntry.CoverUrl; // Stel de cover-URL in
        //    }
        //    _lstMovies.Add(movie);
        //}

        public void AddMovieByMovie(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            //_lstMovies.Add(movie);
            _dbContext.Add(movie);            
        }

        //public void AddMovieByBarcode(string barcode)
        //{
        //    bool blMovieFound;

        //    blMovieFound = false;

        //    foreach (Movie movie in _lstMovies)
        //    {
        //        if (movie.Barcode == barcode)
        //        {
        //            _lstMovies.Add(movie);
        //            blMovieFound = true;
        //        }

        //    }

        //    if (blMovieFound == false)
        //    {

        //        throw new ArgumentException("Movie with given barcode not found.");
        //    }

        //}

        public void AddMovieByBarcode(string barcode)
        {
            // first check to see if the barcode is null or empty.
            if (string.IsNullOrEmpty(barcode))
            {
                throw new ArgumentNullException(nameof(barcode), "barcode cannot be null or empty");
            }

            // Check to see if the movie is already in the list or not.
            Movie movie = _lstMovies.FirstOrDefault (movie1 => movie1.Barcode == barcode); // to retrieve a single object. In this case, a film with a specific barcode.

            if (movie == null)
            {
                throw new ArgumentNullException("Movie with the given barcode not found in the list");
            }

            // Add the movie that is not yet in the list.
            if (!_lstMovies.Contains(movie))
            {
                _lstMovies.Add(movie);
            }
            else
            {
                throw new InvalidOperationException("Movie with the given barcode is already in the list.");
            }

        }

        //public void RemoveMovieByMovie(Movie movie)
        //{
        //    _lstMovies.Remove(movie);
        //}

        public void RemoveMovieByMovie(Movie movie)
        {
            if (movie == null)
                throw new ArgumentNullException(nameof(movie));

            //_lstMovies.Remove(movie);
            _dbContext.Remove(movie);            
        }

        //public void RemoveMovieByBarcode(string barcode)
        //{
        //    bool bMovieFound;

        //    bMovieFound = false;


        //    foreach (Movie movie in _lstMovies)
        //    {
        //        if (movie.Barcode == barcode)
        //        {
        //            _lstMovies.Remove(movie);
        //            bMovieFound = true;
        //        }

        //    }

        //    if (bMovieFound == false)
        //    {
        //        throw new ArgumentException("Movie with given barcode not found.");
        //    }
        //}

        public void RemoveMovieByBarcode(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
                throw new ArgumentNullException(nameof(barcode), "barcode cannot be null or empty");

            Movie movie = _lstMovies.FirstOrDefault (movie1 => movie1.Barcode == barcode); // to retrieve a single object. In this case, a film with a specific barcode.

            if (movie == null)
            {
                throw new ArgumentException("Movie with the given barcode not found.");
            }

            // Remove movie.
            _lstMovies.Remove(movie);
        }

        #endregion

        #region Retrieve movies by status

        //public void ShowAllMoviesAUserContains()
        //{
        //    foreach (Movie movie in _lstMovies)
        //    {
        //        Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}");
        //    }
        //}

        public IEnumerable<Movie> ShowAllMoviesAUserContains()
        {
            return _lstMovies;
        }

        //public void ShowAllMoviesThatHaveBeenSeen()
        //{
        //    foreach(Movie movie in _lstMovies)
        //    {
        //        if (movie.Seen)
        //        {
        //            Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}");
        //        }

        //    }

        //}

        public IEnumerable<Movie> ShowAllMoviesThatHaveBeenSeen()
        {
            return _lstMovies.Where(movie => movie.Seen);
        }

        //public void ShowAllMoviesThatHaveNotBeenSeen()
        //{
        //    foreach (Movie movie in _lstMovies)
        //    {
        //        if (!movie.Seen)
        //        {
        //            Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}");
        //        }

        //    }
        //}

        public IEnumerable<Movie> ShowAllMoviesThatHaveNotBeenSeen()
        {
            return _lstMovies.Where(movie => !movie.Seen);
        }

        #endregion

        #region Filter by movie data

        //public void ShowAllActorsFromMovie(Movie movie)
        //{
        //    foreach (Actor actor in movie.Actors)
        //    {
        //        Console.WriteLine($"Actor: {actor.Name}");
        //    }

        //}

        public IEnumerable<Movie> ShowAllActorsFromMovie(Movie movie)
        {
            if (movie is null)
            {
                throw new ArgumentNullException(nameof(movie), "The movie you are looking for was not found in the list.");
            }

            return (IEnumerable<Movie>)movie.Actors;
        }

        //public void ShowDirectorFromMovie(Movie movie)
        //{
        //    Console.WriteLine($"director: {movie.Director.Name}");
        //}

        public Director ShowDirectorFromMovie(Movie movie)
        {
            if (movie is null)
            {
                throw new ArgumentNullException(nameof(movie), "The movie you are looking for was not found in the list.");
            }

            return movie.Director;
        }

        //public void ShowYearFromMovie(Movie movie)
        //{
        //    Console.WriteLine($"Year: {movie.Year}");
        //}

        public string ShowYearFromMovie(Movie movie)
        {
            if (movie is null)
            {
                throw new ArgumentNullException(nameof(movie), "The movie you are looking for was not found in the list.");
            }

            return movie.Year;
        }

        #endregion

        #region Filter by model

        //public void ShowMoviesFromTheSameActor(Actor actor)
        //{
        //    foreach (Movie movie in _lstMovies)
        //    {
        //        if (movie.Actors.Contains(actor))
        //        {
        //            Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}");
        //        }

        //    }

        //}

        public IEnumerable<Movie> ShowMoviesFromTheSameActor(Actor actor)
        {
            if (actor is null)
            {
                throw new ArgumentNullException(nameof(actor), "The actor you want to search for was not found in the list.");
            }

            return _lstMovies.Where(movie1 => movie1.Actors.Contains(actor));
        }

        //public void ShowMoviesFromTheSameDirector(Director director)
        //{
        //    foreach (Movie movie in _lstMovies)
        //    {
        //        if (movie.Director.Equals(director))
        //        {
        //            Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}");
        //        }

        //    }

        //}

        public IEnumerable<Movie> ShowMoviesFromTheSameDirector(Director director)
        {
            if (director is null)
            {
                throw new ArgumentNullException(nameof(director), "The director you want to search for was not found in the list.");
            }

            return _lstMovies.Where(movie1 => movie1.Director.Equals(director));
        }

        //public void ShowAllMoviesFromTheSameYear(string strYear)
        //{
        //    foreach (Movie movie in _lstMovies)
        //    {
        //        if (movie.Year.Equals(strYear))
        //        {
        //            Console.WriteLine($"Title: {movie.Title}, Director: {movie.Director.Name}");
        //        }

        //    }

        //}

        public IEnumerable<Movie> ShowAllMoviesFromTheSameYear(string strYear)
        {
            if (string.IsNullOrEmpty(strYear))
            {
                throw new ArgumentNullException(nameof(strYear), "The year to search for was not found in the list.");
            }

            return _lstMovies.Where(movie1 => movie1.Year.Equals(strYear));
        }

        #endregion

    }

}
