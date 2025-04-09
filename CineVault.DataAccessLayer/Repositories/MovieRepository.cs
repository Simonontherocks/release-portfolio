using CineVault.DataAccessLayer.Context;
using CineVault.ModelLayer.ModelMovie;
using CineVault.ModelLayer.ModelUser;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.DataAccessLayer.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        #region Field

        private AppDBContext _dbContext;

        #endregion

        #region Constructor

        public MovieRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Check for existing movies

        public bool CheckIfMovieExists (Movie movie)
        {
            return _dbContext.Movies.Any(m => m.IMDBId.Equals(movie.IMDBId));
        }

        #endregion

        #region Adding or removing movies

        public void AddMovieByMovie(Movie movie)
        {

            _dbContext.Add(movie);
            _dbContext.SaveChanges();
        }

        public void RemoveMovieByMovie(Movie movie)
        {
            _dbContext.Remove(movie);
            _dbContext.SaveChanges();
        }

        #endregion

        #region Retrieve movies by status

        public IEnumerable<Movie> ShowAllMoviesAUserContains()
        {
            return _dbContext.Movies.ToList();
        }

        public IEnumerable<Movie> ShowAllMoviesThatHaveBeenSeen()
        {
            return _dbContext.Movies.Where(movie => movie.Seen == true).ToList();
        }

        public IEnumerable<Movie> ShowAllMoviesThatHaveNotBeenSeen()
        {
            return _dbContext.Movies.Where(movie => movie.Seen == false).ToList();
        }

        #endregion

        #region Filter by movie data

        public IEnumerable<Actor> ShowAllActorsFromMovie(Movie movie)
        {
            return _dbContext.MovieActors
            .Where(ma => ma.MovieId == movie.Id)
            .Select(ma => ma.Actor)
            .ToList();
        }

        public IEnumerable<Director> ShowDirectorFromMovie(Movie movie)
        {
            return _dbContext.MovieDirectors
           .Where(md => md.MovieId == movie.Id)
           .Select(md => md.Director)
           .ToList();
        }

        public string ShowYearFromMovie(Movie movie)
        {
            return movie.Year.ToString();
        }

        #endregion

        #region Filter by model

        public IEnumerable<Movie> ShowMoviesFromTheSameActor(Actor actor)
        {
            return _dbContext.MovieActors
                .Where(movieActor => movieActor.Id == actor.Id)
                .Select(movieActor => movieActor.Movie)
                .ToList();
        }

        public IEnumerable<Movie> ShowMoviesFromTheSameDirector(Director director)
        {
            return _dbContext.MovieDirectors
                .Where(movieDirector => movieDirector.Id == director.Id)
                .Select(movieDirector => movieDirector.movie)
                .ToList();
        }

        public IEnumerable<Movie> ShowAllMoviesFromTheSameYear(string strYear)
        {
            if (int.TryParse(strYear, out int year))
            {
                return _dbContext.Movies
                    .Where(movie => movie.Year == strYear)
                    .ToList();
            }

            return Enumerable.Empty<Movie>();
        }

        #endregion

        #region Default CRUD

        public Movie GetById(int id)
        {
            return _dbContext.Movies.FirstOrDefault(m => m.Id == id);
        }

        #endregion

    }

}
