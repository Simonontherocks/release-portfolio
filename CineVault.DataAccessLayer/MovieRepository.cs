using CineVault.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.DataAccessLayer
{
    public class MovieRepository : IMovieRepository
    {
        private readonly List<Movie> _lstMovies = new List<Movie>();
        private readonly List<Actor> _lstActors = new List<Actor>();
        private readonly List<Director> _lstDirectors = new List<Director>();

        #region Adding or removing movies

        public void AddMovieByMovie(Movie movie)
        {
            _lstMovies.Add(movie);
        }

        public void AddMovieByBarcode(string barcode)
        {
            bool blMovieFound;

            blMovieFound = false;

            foreach (Movie movie in _lstMovies)
            {
                if (movie.Barcode == barcode)
                {
                    _lstMovies.Add(movie);
                    blMovieFound = true;
                }

            }

            if (blMovieFound == false)
            {

                throw new ArgumentException("Movie with given barcode not found.");
            }

        }

        public void RemoveMovieByMovie(Movie movie)
        {
            _lstMovies.Remove(movie);
        }

        public void RemoveMovieByBarcode(string barcode)
        {
            bool bMovieFound;

            bMovieFound = false;


            foreach (Movie movie in _lstMovies)
            {
                if (movie.Barcode == barcode)
                {
                    _lstMovies.Remove(movie);
                    bMovieFound = true;
                }

            }

            if (bMovieFound == false)
            {
                throw new ArgumentException("Movie with given barcode not found.");
            }
        }

        #endregion

        #region Showing all movies a user contains

        public void ShowAllMoviesAUserContains()
        {
            foreach (Movie movie in _lstMovies)
            {
                Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}");
            }
        }

        #endregion

        #region Filter by seen or not seen



        public void ShowAllMoviesThatHaveBeenSeen()
        {
            foreach(Movie movie in _lstMovies)
            {
                if (movie.Seen)
                {
                    Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}");
                }

            }

        }

        public void ShowAllMoviesThatHaveNotBeenSeen()
        {
            foreach (Movie movie in _lstMovies)
            {
                if (!movie.Seen)
                {
                    Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}");
                }

            }
        }

        #endregion

        #region Filter by movie data

        public void ShowAllActorsFromMovie(Movie movie)
        {
            foreach (Actor actor in movie.Actors)
            {
                Console.WriteLine($"Actor: {actor.Name}");
            }

        }

        public void ShowDirectorFromMovie(Movie movie)
        {
            Console.WriteLine($"director: {movie.Director.Name}");
        }

        public void ShowYearFromMovie(Movie movie)
        {
            Console.WriteLine($"Year: {movie.Year}");
        }

        #endregion

        #region Filter by model

        public void ShowMoviesFromTheSameActor(Actor actor)
        {
            foreach(Movie movie in _lstMovies)
            {
                if(movie.Actors.Contains(actor))
                {
                    Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}");
                }

            }

        }

        public void ShowMoviesFromTheSameDirector(Director director)
        {
            foreach(Movie movie in _lstMovies)
            {
                if(movie.Director.Equals(director))
                {
                    Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}");
                }

            }

        }

        public void ShowAllMoviesFromTheSameYear(string strYear)
        {
            foreach(Movie movie in _lstMovies)
            {
                if(movie.Year.Equals(strYear))
                {
                    Console.WriteLine($"Title: {movie.Title}, Director: {movie.Director.Name}");
                }

            }

        }

        #endregion

    }

}
