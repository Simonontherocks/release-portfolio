using CineVault.DataAccessLayer.Context;
using CineVault.ModelLayer.ModelMovie;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                return _dbContext.Movies.Any(m => m.TMDBId.Equals(movie.TMDBId));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckIfMovieExists: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Adding or removing movies

        public void AddMovieByMovie(Movie movie)
        {
            try
            {
                _dbContext.Add(movie);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddMovieByMovie: {ex.Message}");
            }
        }

        public void RemoveMovieByMovie(Movie movie)
        {
            try
            {
                _dbContext.Remove(movie);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RemoveMovieByMovie: {ex.Message}");
            }
        }

        #endregion

        #region Retrieve movies by status

        public IEnumerable<Movie> ShowAllMoviesAUserContains()
        {
            try
            {
                return _dbContext.Movies.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAllMoviesAUserContains: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        public IEnumerable<Movie> ShowAllMoviesThatHaveBeenSeen()
        {
            try
            {
                return _dbContext.Movies.Where(movie => movie.Seen == true).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAllMoviesThatHaveBeenSeen: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        public IEnumerable<Movie> ShowAllMoviesThatHaveNotBeenSeen()
        {
            try
            {
                return _dbContext.Movies.Where(movie => movie.Seen == false).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAllMoviesThatHaveNotBeenSeen: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        #endregion

        #region Filter by movie data

        public IEnumerable<Actor> ShowAllActorsFromMovie(Movie movie)
        {
            try
            {
                return _dbContext.MovieActors
                   .Where(ma => ma.MovieId == movie.Id)
                   .Select(ma => ma.Actor)
                   .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAllActorsFromMovie: {ex.Message}");
                return Enumerable.Empty<Actor>();
            }
        }

        public IEnumerable<Director> ShowDirectorFromMovie(Movie movie)
        {
            try
            {
                return _dbContext.MovieDirectors
                  .Where(md => md.MovieId == movie.Id)
                  .Select(md => md.Director)
                  .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowDirectorFromMovie: {ex.Message}");
                return Enumerable.Empty<Director>();
            }
        }

        public string ShowYearFromMovie(Movie movie)
        {
            try
            {
                return movie.Year.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowYearFromMovie: {ex.Message}");
                return string.Empty;
            }
        }

        #endregion

        #region Filter by model

        public IEnumerable<Movie> ShowMoviesFromTheSameActor(Actor actor)
        {
            try
            {
                return _dbContext.MovieActors
                        .Where(movieActor => movieActor.Id == actor.Id)
                        .Select(movieActor => movieActor.Movie)
                        .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowMoviesFromTheSameActor: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        public IEnumerable<Movie> ShowMoviesFromTheSameDirector(Director director)
        {
            try
            {
                return _dbContext.MovieDirectors
                        .Where(movieDirector => movieDirector.Id == director.Id)
                        .Select(movieDirector => movieDirector.movie)
                        .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowMoviesFromTheSameDirector: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        public IEnumerable<Movie> ShowAllMoviesFromTheSameYear(string strYear)
        {
            try
            {
                if (int.TryParse(strYear, out int year))
                {
                    return _dbContext.Movies
                        .Where(movie => movie.Year == strYear)
                        .ToList();
                }

                return Enumerable.Empty<Movie>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAllMoviesFromTheSameYear: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        #endregion

        #region Default CRUD

        public Movie GetById(int id)
        {
            return _dbContext.Movies.Include(m => m.MovieActors) // Hier laad ik de klasse MovieActors vanuit de klasse movie in.
                .ThenInclude(a => a.Actor) // Hier laad ik de acteurs van de film in.
                .Include(m => m.MovieDirectors) // Hier laad ik de klasse MovieDirectors vanuit de klasse movie in.
                .ThenInclude(d => d.Director).FirstOrDefault(m => m.Id == id); // hier laad ik de directors van de film in.
        }

        #endregion

    }

}
