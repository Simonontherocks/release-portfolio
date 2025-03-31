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
        private HttpClient _httpClient; // Deze HttpClient zal gebruikt worden om een verzoek te sturen naar de nodige API.
        readonly string apiKey = "6ff2d9c5916b798dc04c3762ace90261"; // API-sleutel, nodig voor authenticatie bij de API.
        readonly string searchUrl = "https://api.themoviedb.org/3/search/movie?api_key="; // URL voor de zoekopdracht naar films via de API.


        // Constructor voor de ApiService klasse. Dit maakt een nieuwe instance van HttpClient aan.
        public ApiService()
        {
            _httpClient = new HttpClient(); // Initialisatie van de HttpClient om verzoeken te sturen.
        }

        /// <summary>
        /// Deze methode zoekt films op basis van een meegegeven parameter(strTitle)
        /// De methode zal via de api de film(s) zoeken die de meegegeven parameter bevatten.
        /// Het resultaat bevat een lijst van films die aan de zoekcriteria voldoen, inclusief paginering als er meer resultaten zijn.
        /// </summary>
        /// <param name="strTitle">De titel van de film waarop gezocht moet worden.</param>
        /// <returns>Een lijst van films die voldoen aan de zoekcriteria of null bij fout.</returns>

        public async Task<List<Movie>?> GetMoviesByTitle(string strTitle)
        {
            // Controleer of de opgegeven titel niet null of leeg is.
            if (string.IsNullOrEmpty(strTitle))
            {
                throw new ArgumentNullException("Titel mag niet leeg of null zijn", nameof(strTitle)); // Gooi een uitzondering als de titel ongeldig is.
            }

            List<Movie> allMovies = new List<Movie>(); // Lijst om alle films op te slaan die door de API worden gevonden.
            int currentPage = 1; // Start met de eerste pagina.
            int totalPages = 1; // Voor het geval de API geen totalPages teruggeeft, instellen op 1 als standaard.

            do
            {
                // Maak de URL voor de huidige paginering, inclusief de zoekterm en paginanummer.
                string url = $"{searchUrl}{apiKey}&query={strTitle}&page={currentPage}";
                HttpResponseMessage searchResponse = await _httpClient.GetAsync(url); // Verstuur een GET-verzoek naar de API.

                // Controleer of de API een succesvolle respons heeft gegeven.
                if (searchResponse.IsSuccessStatusCode)
                {
                    // Lees de JSON-reactie van de API.
                    string searchJsonResponse = await searchResponse.Content.ReadAsStringAsync();

                    // Deserialize de JSON-reactie naar een MovieResponse-object.
                    MovieResponse? movieResponse = JsonSerializer.Deserialize<MovieResponse>(searchJsonResponse);

                    // Controleer of er resultaten zijn in de API-respons.
                    if (movieResponse != null && movieResponse.Results.Any())
                    {
                        allMovies.AddRange(movieResponse.Results); // Voeg de gevonden films toe aan de lijst.
                        totalPages = movieResponse.TotalPages; // Werk het totale aantal pagina's bij op basis van de respons.
                        currentPage++; // Ga naar de volgende pagina.
                    }
                    else
                    {
                        totalPages = currentPage - 1; // Stop de lus als er geen resultaten meer zijn.
                    }
                }
                else
                {
                    // Gooi een uitzondering als de API een fout retourneert.
                    throw new HttpRequestException("Fout bij het ophalen van de filmgegevens.");
                }
            }
            while (currentPage <= totalPages); // Blijf door pagineren tot alle pagina's zijn doorlopen.

            // Controleer of er films zijn gevonden en toon het aantal in de console.
            if (allMovies.Any())
            {
                Console.WriteLine($"Totaal aantal gevonden films: {allMovies.Count}");
            }
            else
            {
                Console.WriteLine("Geen films gevonden."); // Toon een bericht als er geen resultaten zijn.
            }

            // Retourneer de lijst met films, of null als er geen resultaten zijn.
            return allMovies.Any() ? allMovies : null;
        }

        /// <summary>
        /// Verkrijg de acteurs en regisseurs van een film op basis van de film-ID.
        /// </summary>
        /// <param name="intChosenMovieId">De ID van de gekozen film</param>
        /// <returns>Een dictionary met de naam van de acteurs en regisseurs en hun functies in de film</returns>

        public async Task<Dictionary<int, Dictionary<string, List<string>>>> GetActorsAndDirectorsFromMovie(int intChosenMovieId)
        {
            // Haalt credits op voor de gekozen film door middel van de URL.
            string creditsUrl = $"https://api.themoviedb.org/3/movie/{intChosenMovieId}/credits?api_key={apiKey}";
            HttpResponseMessage creditsResponse = await _httpClient.GetAsync(creditsUrl); // Verstuurt een GET-verzoek om de credits van de film op te halen.
            if (creditsResponse.IsSuccessStatusCode) // Voorwaarde die Controleert of het verzoek succesvol was.
            {
                string creditsJsonResponse = await creditsResponse.Content.ReadAsStringAsync(); // Haalt het JSON-antwoord op van de API.
                MovieCreditsResponse? movieCreditsResponse = JsonSerializer.Deserialize<MovieCreditsResponse>(creditsJsonResponse); // Deserializeert de JSON naar een MovieCreditsResponse-object.

                if (movieCreditsResponse != null) // Als de response succesvol is gedeserializeerd, wordt onderstaande code uitgevoerd.
                {
                    // dictionary = <int (imdb-id van persoon), dictionary<string(naam van persoon), List<string (functies in de film)>>>
                    Dictionary<int, Dictionary<string, List<string>>> secondCastAndCrew = new Dictionary<int, Dictionary<string, List<string>>>();

                    // Toevoegen van acteurs

                    foreach (Cast cast in movieCreditsResponse.Cast)
                    {
                        Dictionary<string, List<string>> personDictionary = new Dictionary<string, List<string>>
                        {
                            { cast.Name, new List<string> { "actor" } }
                        };

                        secondCastAndCrew[cast.Imdb_Id] = personDictionary;
                    }

                    // Toevoegen van regisseurs

                    foreach (Crew crew in movieCreditsResponse.Crew)
                    {
                        if (crew.Job == "Director")
                        {
                            if (secondCastAndCrew.ContainsKey(crew.Imdb_Id))
                            {
                                secondCastAndCrew[crew.Imdb_Id][crew.Name].Add("director");
                            }
                            else
                            {
                                Dictionary<string, List<string>> personDictionary = new Dictionary<string, List<string>>
                                {
                                    { crew.Name, new List<string> { "director" } }
                                };

                                secondCastAndCrew[crew.Imdb_Id] = personDictionary;
                            }

                        }

                    }

                     return secondCastAndCrew;
                }

            }

            // Als er een fout is opgetreden of geen credits worden gevonden, wordt er null gereturned.
            return null;

        }

    }

}
