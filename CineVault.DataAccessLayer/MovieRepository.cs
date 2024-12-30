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
        public MovieRepository() 
        {
            //Default constructor
        }

        #region Adding or removing movies

        public void AddMovieByMovie(Movie movie)
        {
            
        }

        public void AddMovieByBarcode(string barcode)
        {
            throw new NotImplementedException();
        }

        public void RemoveMovieByMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public void RemoveMovieByBarcode(string barcode)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Filter by seen or not seen

        public void ShowAllMoviesThatHaveBeenSeen()
        {
            throw new NotImplementedException();
        }

        public void ShowAllMoviesThatHaveNotBeenSeen()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Filter by movie data

        public void ShowAllActorsFromMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public void ShowDirectorFromMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public void ShowYearFromMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Filter by model

        public void ShowMoviesFromTheSameActor(Actor actor)
        {
            throw new NotImplementedException();
        }

        public void ShowMoviesFromTheSameDirector(Director director)
        {
            throw new NotImplementedException();
        }

        public void ShowAllMoviesFromTheSameYear(string strYear)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
