using CineVault.BusinessLogic.Service;
using CineVault.DataAccessLayer;
using CineVault.PresentationLayor.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CineVault.PresentationLayor.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, MovieService repo)
        {
            _logger = logger;
            repo.RemoveMovieByMovie(new ModelLayer.ModelMovie.Movie() { Id = 7 });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
