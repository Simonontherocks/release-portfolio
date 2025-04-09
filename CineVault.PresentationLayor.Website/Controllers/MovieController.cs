using Microsoft.AspNetCore.Mvc;
using CineVault.BusinessLogic.Service;
using CineVault.ModelLayer.ModelMovie;

namespace CineVault.PresentationLayer.Website.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieService _movieService;
        private readonly ActorService _actorService;
        private readonly DirectorService _directorService;

        public MovieController(MovieService movieService, ActorService actorService, DirectorService directorService)
        {
            _movieService = movieService;
            _actorService = actorService;
            _directorService = directorService;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Mijn films acties

        public async Task<IActionResult> AllMovies()
        {
            // IActionResult verwacht hier een view terug en dus wordt de lijst die in de view teruggeven moet worden, tussen de haken van de view gestoken.
            // Hier moet het niet async zijn, omdat in de service await wordt gebruikt.

            return View("Index", await _movieService.ShowAllMovies());
        }

        public async Task<IActionResult> AllSeenMovies()
        {
            return View("Index", await _movieService.ShowAllMoviesThatHaveBeenSeen());
        }

        public async Task<IActionResult> AllUnseenMovies()
        {
            return View("Index", await _movieService.ShowAllMoviesThatHaveNotBeenSeen());
        }

        #endregion

        #region Zoek acties

        public IActionResult SearchByActor()
        {
            // Ik vraag eerst een lijst van alle acteurs die in de database zitten op.
            return View(_actorService.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"> is de acteur-ID van mijn databank.</param> 
        /// <returns></returns>
        public async Task<IActionResult> ShowMoviesFromActor(int id)
        {
            Actor searchedActor = _actorService.GetById(id);
            return View(await _movieService.ShowMoviesFromTheSameActorAsync(searchedActor));
        }

        public IActionResult SearchByDirector()
        {
            return View(_directorService.GetAll());
        }

        public async Task<IActionResult> ShowMoviesFromDirector(int id)
        {
            Director searchedDirector = _directorService.GetById(id);
            return View(await _movieService.ShowMoviesFromTheSameDirectorAsync(searchedDirector));
        }

        public IActionResult SearchByYear()
        {
            return View(_movieService.GetAllYears());
        }

        public async Task<IActionResult> ShowMoviesFromYear(int id)
        {
            Movie searchedMovie = _movieService.GetById(id);
            return View(await _movieService.ShowAllMoviesFromTheSameYearAsync(searchedMovie.Year));
        }

        #endregion

    }

}
