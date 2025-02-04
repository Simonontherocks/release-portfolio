using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineVault.DataAccessLayer;
using CineVault.ModelLayer.ModelMovie;

namespace CineVault.BusinessLogic.Service
{
    public class MovieService
    {
        #region Field

        private readonly IMovieRepository _movieRepository;

        #endregion


        // under construction
        #region Constructor

        //public MovieRepository(IMovieRepository movieRepository)
        //{
        //    _movieRepository = movieRepository;
        //}

        #endregion


        #region Methods

        #region Adding or removing movies

        public void AddMovieByMovie(Movie movie)
        {

            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie), "movie cannot be NULL");
            }

            _movieRepository.AddMovieByMovie(movie);
        }

        public void RemoveMovieByMovie(Movie movie)
        {

            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie), "movie cannot be NULL");
            }

            _movieRepository.RemoveMovieByMovie(movie);
        }

        #endregion

        #region Retrieve movies by status

        public IEnumerable<Movie> ShowAllMoviesAUserContains()
        {
            return _movieRepository.ShowAllMoviesAUserContains();
        }

        public IEnumerable<Movie> ShowAllMoviesThatHaveBeenSeen()
        {
            return _movieRepository.ShowAllMoviesThatHaveBeenSeen();
        }

        public IEnumerable<Movie> ShowAllMoviesThatHaveNotBeenSeen()
        {
            return _movieRepository.ShowAllMoviesThatHaveNotBeenSeen();
        }

        #endregion

        #region Filter by movie data

        public IEnumerable<Actor> ShowAllActorsFromMovie(Movie movie)
        {

            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            return _movieRepository.ShowAllActorsFromMovie(movie);
        }

        public IEnumerable<Director> ShowDirectorFromMovie(Movie movie)
        {

            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            return _movieRepository.ShowDirectorFromMovie(movie);
        }

        public string ShowYearFromMovie(Movie movie)
        {

            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            return _movieRepository.ShowYearFromMovie(movie);
        }

        #endregion

        #region Filter by model

        public IEnumerable<Movie> ShowMoviesFromTheSameActor(Actor actor)
        {

            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            return _movieRepository.ShowMoviesFromTheSameActor(actor);
        }

        public IEnumerable<Movie> ShowMoviesFromTheSameDirector(Director director)
        {

            if (director == null)
            {
                throw new ArgumentNullException(nameof(director));
            }

            return _movieRepository.ShowMoviesFromTheSameDirector(director);
        }

        public IEnumerable<Movie> ShowAllMoviesFromTheSameYear(string strYear)
        {

            if (string.IsNullOrWhiteSpace(strYear))
            {
                throw new ArgumentException("Year cannot be null or empty", nameof(strYear));
            }                

            return _movieRepository.ShowAllMoviesFromTheSameYear(strYear);
        }

        #endregion

        #endregion

    }
}
