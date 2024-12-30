using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineVault.BusinessLogic.Models;

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
        public void ShowAllMoviesAUserContains();
        public void ShowAllMoviesThatHaveBeenSeen();
        public void ShowAllMoviesThatHaveNotBeenSeen();

        // This is where data from a movie is filtered.
        public void ShowAllActorsFromMovie(Movie movie);
        public void ShowDirectorFromMovie(Movie movie);
        public void ShowYearFromMovie(Movie movie);

        // This filters on a model and returns a list.
        public void ShowMoviesFromTheSameActor(Actor actor);
        public void ShowMoviesFromTheSameDirector(Director director);
        public void ShowAllMoviesFromTheSameYear(string strYear);

    }
}
