using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CineVault.BusinessLogic.ApiModels;
using CineVault.ModelLayer;
using CineVault.ModelLayer.ModelMovie;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CineVault.BusinessLogic.Service
{
    public class ApiService
    {
        private HttpClient _httpClient;
        readonly string apiKey = "6ff2d9c5916b798dc04c3762ace90261";
        readonly string searchUrl = "https://api.themoviedb.org/3/search/movie?api_key="; //{apiKey}&query={Uri.EscapeDataString(query)}";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }
        
        /// <summary>
        /// Hier zal een film gezocht worden op basis van een meegegeven parameter.
        /// De methode zal via de api de film(s) zoeken die de meegegeven parameter bevatten.
        /// Daarna zullen alle films getoont worden op de console.
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns></returns>
        public async Task<List<Movie>?> GetMoviesByTitle(string strTitle)
        {
            HttpResponseMessage searchResponse = await _httpClient.GetAsync(searchUrl + apiKey + $"&query={strTitle}");

            if (searchResponse.IsSuccessStatusCode) // voert uit als het true is.
            {
                string searchJsonRespons = await searchResponse.Content.ReadAsStringAsync();
                MovieResponse? movieResponse = JsonSerializer.Deserialize<MovieResponse>(searchJsonRespons);

                if (movieResponse != null)
                {
                    Debug.WriteLine("Gevonden films");
                    foreach (CineVault.ModelLayer.ModelMovie.Movie movie in movieResponse.Results)
                    {
                        Debug.WriteLine($"ID: {movie.Id}, Titel: {movie.Title}, Jaar: {movie.Year?.Split('-')[0]}");
                        // de releasedate zal gesplits worden dmv de Split()-methode.
                        // de split zal gebeuren op basis van het "-" teken en in een array gestoken worden.
                        // Doordat de datum van api wordt verkregen door jaar - maand - dag
                        // En omdat ik enkel het eerste object opvraag, zal ik enkel het jaartal krijgen.

                    }
                    
                    return movieResponse.Results;

                }

            }

            return null; // Ik geef een lege lijst terug als de API-response niet correct was.
        }

        public async Task<Dictionary<string,List<string>>?> GetActorsAndDirectorsFromMovieId(int intChosenMovieId)
        {
            // Console.WriteLine("\nVoer de film-ID in van de gewenste film:");
            // int chosenMovieId = int.Parse(Console.ReadLine());

            // Haal credits op voor de gekozen film
            string creditsUrl = $"https://api.themoviedb.org/3/movie/{intChosenMovieId}/credits?api_key={apiKey}";
            HttpResponseMessage creditsResponse = await _httpClient.GetAsync(creditsUrl);
            if(creditsResponse.IsSuccessStatusCode)
            {
                string creditsJsonResponse = await creditsResponse.Content.ReadAsStringAsync();
                MovieCreditsResponse? movieCreditsResponse = JsonSerializer.Deserialize<MovieCreditsResponse>(creditsJsonResponse);

                if (movieCreditsResponse != null)
                {
                    // dictionary = <string (naam van de persoon)<List(functies in de film van de persoon)>>
                    Dictionary<string, List<string>> dictionaryOfCastAndCrews = new Dictionary<string, List<string>>();
                    foreach (Cast cast in movieCreditsResponse.Cast)
                    {
                        dictionaryOfCastAndCrews.Add(cast.Name, new List<string> { "actor" });
                    }

                    foreach (Crew crew in movieCreditsResponse.Crew)
                    {
                        if (crew.Job == "Director")
                        {
                            // ToDo: te controleren op dubbele namen en toch verschillende personen.
                            if (dictionaryOfCastAndCrews.Keys.Contains(crew.Name))
                            {
                                dictionaryOfCastAndCrews[crew.Name].Add("director");
                            }
                            else
                            {
                                dictionaryOfCastAndCrews.Add(crew.Name, new List<string> { "director" });
                            }
                            
                        }

                    }

                    return dictionaryOfCastAndCrews;

                }

            }           

            return null;
            
        }

    }

}
