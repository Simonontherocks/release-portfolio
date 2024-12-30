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
        public void AddMovie(Movie movie);
        public void RemoveMovie(Movie movie);

        public void ShowAllMoviesThatHaveBeenSeen();
        public void ShowAllMoviesThatHaveNotBeenSeen();

        public void ShowAllActorsFromMovie(Movie movie);
        public void ShowDirectorFromMovie(Movie movie);
        public void ShowYearFromMovie(Movie movie);

        public void ShowMoviesFromSameActor(Actor actor);
        public void ShowActorsFromSameDirector(Director director);
        

    }
}
