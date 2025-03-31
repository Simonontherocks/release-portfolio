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
        /// Hier zal een film gezocht worden op basis van een meegegeven parameter.
        /// De methode zal via de api de film(s) zoeken die de meegegeven parameter bevatten.
        /// Daarna zullen alle films getoont worden op de console.
        /// </summary>
        /// <param name="strTitle">De titel van de film waar je wilt op zoeken.</param>
        /// <returns>Een lijst van films die voldoen aan de zoekcriteria of null bij fout.</returns>

        public async Task<List<Movie>?> GetMoviesByTitle(string strTitle)
        {
            if (string.IsNullOrEmpty(strTitle))
            {
                throw new ArgumentNullException("Titel mag niet leeg of null zijn", nameof(strTitle));
            }
            // Het HTTP-verzoek om films te zoeken op basis van de titel (searchUrl + apiKey + zoekquery).
            HttpResponseMessage searchResponse = await _httpClient.GetAsync(searchUrl + apiKey + $"&query={strTitle}");

            // Voorwaarde die Controleert of het verzoek succesvol was.
            if (searchResponse.IsSuccessStatusCode) // voert uit als het true is.
            {
                string searchJsonRespons = await searchResponse.Content.ReadAsStringAsync(); // Haalt het JSON-antwoord op van de API.
                MovieResponse? movieResponse = JsonSerializer.Deserialize<MovieResponse>(searchJsonRespons); // Deserializeert de JSON naar een MovieResponse-object.

                if (movieResponse != null && movieResponse.Results.Any()) // Als de response succesvol is gedeserializeerd, wordt onderstaande code uitgevoerd.
                {
                    Debug.WriteLine("Gevonden films"); // Wordt gebruikt om output naar de console te sturen, wat handig is voor debugging.
                    foreach (CineVault.ModelLayer.ModelMovie.Movie movie in movieResponse.Results) // Itereer over de lijst van films die de API heeft teruggegeven.
                    {
                        Debug.WriteLine($"ID: {movie.IMDBId}, Titel: {movie.Title}, Jaar: {movie.Year.Substring(0, 4)}"); // ?.Split('-')[0]}"); // Toon de ID, titel, en jaar van elke film in de console.
                        // de releasedate zal gesplits worden dmv de Split()-methode.
                        // de split zal gebeuren op basis van het "-" teken en in een array gestoken worden.
                        // Doordat de datum van api wordt verkregen door jaar - maand - dag
                        // En omdat ik enkel het eerste object opvraag, zal ik enkel het jaartal krijgen.

                    }

                    return movieResponse.Results; // Geeft de lijst van gevonden films terug.

                }
                else
                {
                    throw new InvalidOperationException("Geen films gevonden met opgegeven titel.");
                }

            }

            throw new HttpRequestException("Fout bij het ophalen van de film");
            // return null; // Ik geef een lege lijst terug als de API-response niet correct was.
        }

        /// <summary>
        /// Verkrijg de acteurs en regisseurs van een film op basis van de film-ID.
        /// </summary>
        /// <param name="intChosenMovieId">De ID van de gekozen film</param>
        /// <returns>Een dictionary met de naam van de acteurs en regisseurs en hun functies in de film</returns>

        //ToDo: nog controleren of alle films van alle pages uit de API meegenomen worden.

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
