using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CineVault.BusinessLogic.ApiModels;
using CineVault.ModelLayer;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CineVault.BusinessLogic.Service
{
    public class ApiService
    {
        readonly string apiKey = "6ff2d9c5916b798dc04c3762ace90261";
        readonly string searchUrl = "https://api.themoviedb.org/3/search/movie?api_key="; //{apiKey}&query={Uri.EscapeDataString(query)}";
        
        /// <summary>
        /// Hier zal een film gezocht worden op basis van een meegegeven parameter.
        /// De methode zal via de api de film(s) zoeken die de meegegeven parameter bevatten.
        /// Daarna zullen alle films getoont worden op de console.
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns></returns>
        public async Task GetMovieByTitle(string strTitle)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage searchResponse = await client.GetAsync(searchUrl + apiKey + $"&query={strTitle}");
            searchResponse.EnsureSuccessStatusCode();
            string searchJsonRespons = await searchResponse.Content.ReadAsStringAsync();
            MovieResponse movieResponse = JsonSerializer.Deserialize<MovieResponse>(searchJsonRespons);

            Console.WriteLine("Gevonden films");
            foreach(CineVault.ModelLayer.ModelMovie.Movie movie in movieResponse.Results)
            {
                Console.WriteLine($"ID: {movie.Id}, Titel: {movie.Title}, Jaar: {movie.Year?.Split('-')[0]}");
                // de releasedate zal gesplits worden dmv de Split()-methode.
                // de split zal gebeuren op basis van het "-" teken en in een array gestoken worden.
                // Doordat de datum van api wordt verkregen door jaar - maand - dag
                // En omdat ik enkel het eerste object opvraag, zal ik enkel het jaartal krijgen.
            }

        }

    }

}
