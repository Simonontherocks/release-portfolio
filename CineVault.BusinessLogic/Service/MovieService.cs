using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineVault.BusinessLogic.ApiModels;
using CineVault.DataAccessLayer.Repositories;
using CineVault.ModelLayer.ModelMovie;
using Microsoft.EntityFrameworkCore;

namespace CineVault.BusinessLogic.Service
{
    /// <summary>
    /// Deze klasse zal contact maken met de DAL.
    /// Deze zal via de methodes hier, data gaan ophalen uit de database.
    /// De methodes in deze klasse hebben dezelfde signatuur als in de DAL, om het leesbaar en eenvoudig te houden.
    /// Er wordt gebruik gemaakt van Dependency Injection in het field, namelijk van deIMovieRepository
    /// </summary>

    public class MovieService
    {
        #region Field

        private readonly IMovieRepository _movieRepository;
        private readonly ApiService _apiService;

        #endregion


        #region Constructor

        public MovieService(IMovieRepository movieRepository, ApiService apiService)
        {
            _movieRepository = movieRepository;
            _apiService = apiService;
        }

        #endregion


        #region Methods

        //#region Check for existing movies

        //public bool MovieExists(string title, string releaseDate)
        //{
        //    return _dbContext.Movies.Any(m => m.Title == title && m.Year == releaseDate);
        //}

        //#endregion

        #region Adding or removing movies


        public void AddMovieByMovie(Movie movie)
        {
            _apiService.GetMoviesByTitle(movie.Title);

            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie), "movie cannot be NULL");
            }

            // ToDo: Deze methode is aangemaakt in de interface en de klasse zelf. Nog controleren of deze methode werkt.
            if(_movieRepository.CheckIfMovieExists(movie.Title, movie.Year))
            {
                throw new InvalidOperationException("A movie with the same title and release date already exists.");
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

        #region Using adapter

        

        #endregion

    }
}
