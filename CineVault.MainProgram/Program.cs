using CineVault.BusinessLogic.Service;
using CineVault.ModelLayer.ModelAbstractClass;
using CineVault.ModelLayer.ModelMovie;
using CineVault.ModelLayer.ModelUser;
using CineVault.ModelLayer.ModelLayerService;
using CineVault.DataAccessLayer;
using System.IO;
using System.Reflection;

namespace CineVault.MainProgram
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ApiService apiService = new ApiService();

            List<Movie>? movieList;
            movieList = await apiService.GetMoviesByTitle("braveheart");
            if (movieList.Count > 0)
            {
                var dict = await apiService.GetActorsAndDirectorsFromMovieId(movieList[0].IMDBId);
            }        
        }

    }

}