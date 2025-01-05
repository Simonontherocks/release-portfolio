using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineVault.BusinessLogic.ModelMovie;

namespace CineVault.DataAccessLayer
{
    public interface IMovieRepository
    {
        // Here you should be able to add or remove a movie.
        public void AddMovieByMovie(Movie movie);
        public void AddMovieByBarcode(string barcode);
        public void RemoveMovieByMovie(Movie movie);
        public void RemoveMovieByBarcode(string barcode);

        // Here you should be able to filter on the films that have been seen and those that have not been seen.
        public IEnumerable<Movie> ShowAllMoviesAUserContains();
        public IEnumerable<Movie> ShowAllMoviesThatHaveBeenSeen();
        public IEnumerable<Movie> ShowAllMoviesThatHaveNotBeenSeen();

        // This is where data from a movie is filtered.
        public IEnumerable<Movie> ShowAllActorsFromMovie(Movie movie);
        public Director ShowDirectorFromMovie(Movie movie);
        public string ShowYearFromMovie(Movie movie);

        // This filters on a model and returns a list.
        public IEnumerable<Movie> ShowMoviesFromTheSameActor(Actor actor);
        public IEnumerable<Movie> ShowMoviesFromTheSameDirector(Director director);
        public IEnumerable<Movie> ShowAllMoviesFromTheSameYear(string strYear);

    }
}
