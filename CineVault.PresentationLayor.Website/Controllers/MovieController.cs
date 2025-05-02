using Microsoft.AspNetCore.Mvc;
using CineVault.BusinessLogic.Service;
using CineVault.ModelLayer.ModelMovie;
using CineVault.PresentationLayer.Website.ViewModels;
using CineVault.PresentationLayor.Website.Models;

namespace CineVault.PresentationLayer.Website.Controllers
{
    public class MovieController : Controller
    {
        #region Field

        private readonly MovieService _movieService;
        private readonly ActorService _actorService;
        private readonly DirectorService _directorService;
        private readonly ApiService _apiService;

        #endregion

        #region Constructor

        public MovieController(MovieService movieService, ActorService actorService, DirectorService directorService, ApiService apiService)
        {
            _movieService = movieService;
            _actorService = actorService;
            _directorService = directorService;
            _apiService = apiService;
        }

        #endregion

        #region Index

        public async Task<IActionResult> Index()
        {
            return View(await _movieService.ShowAllMovies());
        }

        #endregion

        #region Films toevoegen en verwijderen

        public IActionResult AddMovie() // Hier komt men op de pagina om een film toe te voegen.
        {
            return View();
        }

        public async Task<List<ApiMovie>?> GetMoviesFromTmdb(string movieTitle) // Hier zal een filmtitel in de zoekbalk getypt moeten worden en zal er op gezocht worden.
        {
            List<ApiMovie> result= new List<ApiMovie>();
            List<Movie>? apiResult = await _apiService.GetMoviesByTitle(movieTitle);

            if(apiResult is null)
            {
                throw new Exception("Geen films gevonden");
            }
            else
            {
                foreach (Movie movie in apiResult)
                {
                    result.Add(new ApiMovie { Title = movie.Title, TMDBId = movie.TMDBId, Year = movie.Year });
                }
            }
            
            return result;
        }

        public async Task<IActionResult> AddMovieByTmdbId(int tmdbId) // De film, cast eb crew worden aan de hand van het TMDB-Id opgeslaan in de databank.
        {
            try
            {
                await _movieService.AddMovieByTmdbId(tmdbId);
                return Redirect(nameof(AllMovies));
            }
            catch (Exception exception)
            {
                ErrorViewModel error = new ErrorViewModel();
                error.ErrorMessage = exception.Message;
                return View("Error", error);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _movieService.RemoveMovieByIdAsync(id);
            return RedirectToAction("Index");
        }

        #endregion

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
            ViewData["BodyClass"] = "actor-bg";
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
            ViewData["BodyClass"] = "director-bg";
            // Ik vraag eerst een lijst van alle regisseurs die in de database zitten op.
            return View(_directorService.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"> is de regisseur-ID van mijn databank.</param> 
        /// <returns></returns>
        public async Task<IActionResult> ShowMoviesFromDirector(int id)
        {
            Director searchedDirector = _directorService.GetById(id);
            return View(await _movieService.ShowMoviesFromTheSameDirectorAsync(searchedDirector));
        }

        public IActionResult SearchByYear()
        {
            ViewData["BodyClass"] = "year-bg";
            return View(_movieService.GetAllYears());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"> is de film-ID van mijn databank.</param> 
        /// <returns></returns>
        public async Task<IActionResult> ShowMoviesFromYear(string year)
        {
            return View(await _movieService.ShowAllMoviesFromTheSameYearAsync(year));
        }

        #endregion

        #region Filmstatus veranderen

        // Zet een film op gezien of ongezien

        [HttpPost]
        public async Task<bool> ToggleSeen(int id)
        {
            try
            {
                Movie movie = _movieService.GetById(id);
                await _movieService.SetMovieSeenStatusAsync(id, !movie.Seen);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Details van een film tonen

        public IActionResult Details(int id)
        {
            return View(_movieService.GetById(id)); // Hier geef ik mee van welke film ik de details wil zien.
        }

        #endregion
    }

}
