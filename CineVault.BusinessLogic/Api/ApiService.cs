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
            // Het HTTP-verzoek om films te zoeken op basis van de titel (searchUrl + apiKey + zoekquery).
            HttpResponseMessage searchResponse = await _httpClient.GetAsync(searchUrl + apiKey + $"&query={strTitle}");

            // Voorwaarde die Controleert of het verzoek succesvol was.
            if (searchResponse.IsSuccessStatusCode) // voert uit als het true is.
            {                
                string searchJsonRespons = await searchResponse.Content.ReadAsStringAsync(); // Haalt het JSON-antwoord op van de API.
                MovieResponse? movieResponse = JsonSerializer.Deserialize<MovieResponse>(searchJsonRespons); // Deserializeert de JSON naar een MovieResponse-object.

                if (movieResponse != null) // Als de response succesvol is gedeserializeerd, wordt onderstaande code uitgevoerd.
                {
                    Debug.WriteLine("Gevonden films"); // Wordt gebruikt om output naar de console te sturen, wat handig is voor debugging.
                    foreach (CineVault.ModelLayer.ModelMovie.Movie movie in movieResponse.Results) // Itereer over de lijst van films die de API heeft teruggegeven.
                    {
                        Debug.WriteLine($"ID: {movie.IMDBId}, Titel: {movie.Title}, Jaar: {movie.Year?.Split('-')[0]}"); // Toon de ID, titel, en jaar van elke film in de console.
                        // de releasedate zal gesplits worden dmv de Split()-methode.
                        // de split zal gebeuren op basis van het "-" teken en in een array gestoken worden.
                        // Doordat de datum van api wordt verkregen door jaar - maand - dag
                        // En omdat ik enkel het eerste object opvraag, zal ik enkel het jaartal krijgen.

                    }
                    
                    return movieResponse.Results; // Geeft de lijst van gevonden films terug.

                }

            }

            return null; // Ik geef een lege lijst terug als de API-response niet correct was.
        }

        /// <summary>
        /// Verkrijg de acteurs en regisseurs van een film op basis van de film-ID.
        /// </summary>
        /// <param name="intChosenMovieId">De ID van de gekozen film</param>
        /// <returns>Een dictionary met de naam van de acteurs en regisseurs en hun functies in de film</returns>

        public async Task<Dictionary<string,List<string>>?> GetActorsAndDirectorsFromMovieId(int intChosenMovieId)
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
                    // dictionary = <string (naam van de persoon)<List(functies in de film van de persoon)>>
                    Dictionary<string, List<string>> dictionaryOfCastAndCrews = new Dictionary<string, List<string>>(); // Maak een nieuwe dictionary om acteurs en regisseurs bij te houden.
                    foreach (Cast cast in movieCreditsResponse.Cast) // Voegt de acteurs toe aan de dictionary met hun naam als sleutel en "actor" als functie.
                    {
                        dictionaryOfCastAndCrews.Add(cast.Name, new List<string> { "actor" });
                    }

                    foreach (Crew crew in movieCreditsResponse.Crew) // Voegt de regisseurs toe aan de dictionary. Als een regisseur al bestaat, wordt "director" toegevoegd aan hun functies.
                    {
                        if (crew.Job == "Director")
                        {
                            // ToDo: te controleren op dubbele namen en toch verschillende personen.
                            if (dictionaryOfCastAndCrews.Keys.Contains(crew.Name)) // Als de regisseur al in de dictionary staat, voegt di de functie "director" toe.
                            {
                                dictionaryOfCastAndCrews[crew.Name].Add("director");
                            }
                            else // Voegt de regisseur toe aan de dictionary als deze nog niet bestaat.
                            {
                                dictionaryOfCastAndCrews.Add(crew.Name, new List<string> { "director" });
                            }
                            
                        }

                    }

                    // Return de dictionary met de acteurs en regisseurs en hun functies.
                    return dictionaryOfCastAndCrews;

                }

            }

            // Als er een fout is opgetreden of geen credits worden gevonden, wordt er null gereturned.
            return null;
            
        }

    }

}
