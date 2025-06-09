using System.Diagnostics;
using System.Text.Json;
using CineVault.BusinessLogic.ApiModels;
using CineVault.ModelLayer.ModelMovie;

namespace CineVault.BusinessLogic.Service
{
    /// <summary>
    /// - Deze klasse verzorgt communicatie met de TMDb API.
    /// - Ze biedt functionaliteit voor het zoeken van films, ophalen van filmgegevens, 
    /// - en het verkrijgen van cast en crew informatie.
    /// </summary>

    public class ApiService
    {
        private HttpClient _httpClient; // Deze HttpClient zal gebruikt worden om een verzoek te sturen naar de nodige API.
        readonly string apiKey = "6ff2d9c5916b798dc04c3762ace90261"; // API-sleutel, nodig voor authenticatie bij de API.
        readonly string searchUrl = "https://api.themoviedb.org/3/search/movie?api_key="; // URL voor de zoekopdracht naar films via de API.
        readonly string headerApiKey = "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI2ZmYyZDljNTkxNmI3OThkYzA0YzM3NjJhY2U5MDI2MSIsIm5iZiI6MTczODI0NTEzMy44NDUsInN1YiI6IjY3OWI4NDBkNGViYzk5MWVhNGJkOGE0MCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.lC9tpxspAO6GrYE_Z0tgeL1x6-BppT4sVnxPo13ST1c";

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
            if (string.IsNullOrEmpty(strTitle)) // Controleer of de opgegeven titel niet null of leeg is.
            {
                throw new ArgumentNullException("Titel mag niet leeg of null zijn", nameof(strTitle)); // Gooi een uitzondering als de titel ongeldig is.
            }

            List<Movie> allMovies = new List<Movie>(); // Lijst om alle films op te slaan die door de API worden gevonden.
            int currentPage = 1; // Start met de eerste pagina.
            int totalPages = 1; // Voor het geval de API geen totalPages teruggeeft, instellen op 1 als standaard.

            do
            {                
                string url = $"{searchUrl}{apiKey}&query={strTitle}&page={currentPage}"; // Maak de URL voor de huidige paginering, inclusief de zoekterm en paginanummer.
                HttpResponseMessage searchResponse = await _httpClient.GetAsync(url); // Verstuur een GET-verzoek naar de API.
                                
                if (searchResponse.IsSuccessStatusCode) // Controleer of de API een succesvolle respons heeft gegeven.
                {                    
                    string searchJsonResponse = await searchResponse.Content.ReadAsStringAsync(); // Lees de JSON-reactie van de API.
                                        
                    MovieResponse? movieResponse = JsonSerializer.Deserialize<MovieResponse>(searchJsonResponse); // Deserialize de JSON-reactie naar een MovieResponse-object.
                                        
                    if (movieResponse != null && movieResponse.Results.Any()) // Controleer of er resultaten zijn in de API-respons.
                    {                        
                        foreach (Movie movie in movieResponse.Results) // Deze lus zal ervoor zorgen dat het binnengekregen jaar enkel het jaartal opslaat.
                        {
                            if (!string.IsNullOrEmpty(movie.Year) && movie.Year.Length >= 4)
                            {
                                movie.Year = movie.Year.Substring(0, 4); // enkel het jaar zal overhouden worden.
                            }

                            if (movie.Score.HasValue) // Hier wordt er nagegaan of de score uit de API wel degelijk een waarde heeft.
                            {
                                movie.Score = Math.Round(movie.Score.Value, 2);
                            }

                            Debug.WriteLine($"ID: {movie.TMDBId}, Titel: {movie.Title}, Jaar: {movie.Year}, Score: {movie.Score}");
                        }

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
                    throw new HttpRequestException("Fout bij het ophalen van de filmgegevens."); // Gooi een uitzondering als de API een fout retourneert.
                }
            }
            while (currentPage <= totalPages); // Blijf door pagineren tot alle pagina's zijn doorlopen.
                        
            if (allMovies.Any()) // Controleer of er films zijn gevonden en toon het aantal in de console.
            {
                Debug.WriteLine($"Totaal aantal gevonden films: {allMovies.Count}");
            }
            else
            {
                Debug.WriteLine("Geen films gevonden."); // Toon een bericht als er geen resultaten zijn.
            }
            
            return allMovies.Any() ? allMovies : null; // Retourneer de lijst met films, of null als er geen resultaten zijn.
        }

        /// <summary>
        /// Verkrijg de acteurs en regisseurs van een film op basis van de film-ID.
        /// </summary>
        /// <param name="intChosenMovieId">De ID van de gekozen film</param>
        /// <returns>Een dictionary met de naam van de acteurs en regisseurs en hun functies in de film</returns>

        public async Task<Dictionary<int, Dictionary<string, List<string>>>> AddMovieWithActorsAndDirectorsToDatabase(int intChosenMovieId)
        {            
            string creditsUrl = $"https://api.themoviedb.org/3/movie/{intChosenMovieId}/credits?api_key={apiKey}"; // Haalt credits op voor de gekozen film door middel van de URL.
            HttpResponseMessage creditsResponse = await _httpClient.GetAsync(creditsUrl); // Verstuurt een GET-verzoek om de credits van de film op te halen.
            if (creditsResponse.IsSuccessStatusCode) // Voorwaarde die Controleert of het verzoek succesvol was.
            {
                string creditsJsonResponse = await creditsResponse.Content.ReadAsStringAsync(); // Haalt het JSON-antwoord op van de API.
                MovieCreditsResponse? movieCreditsResponse = JsonSerializer.Deserialize<MovieCreditsResponse>(creditsJsonResponse); // Deserializeert de JSON naar een MovieCreditsResponse-object.

                if (movieCreditsResponse != null) // Als de response succesvol is gedeserializeerd, wordt onderstaande code uitgevoerd.
                {
                    // dictionary = <int (tmdb-id van persoon), dictionary<string(naam van persoon), List<string (functies in de film)>>>
                    Dictionary<int, Dictionary<string, List<string>>> secondCastAndCrew = new Dictionary<int, Dictionary<string, List<string>>>();

                    foreach (Cast cast in movieCreditsResponse.Cast) // Toevoegen van acteurs
                    {
                        Dictionary<string, List<string>> personDictionary = new Dictionary<string, List<string>>
                        {
                            { cast.Name, new List<string> { "actor" } }
                        };

                        secondCastAndCrew[cast.Tmdb_Id] = personDictionary; // Voegt acteur toe.
                    }

                    foreach (Crew crew in movieCreditsResponse.Crew) // Toevoegen van regisseurs
                    {
                        if (crew.Job == "Director")
                        {
                            if (secondCastAndCrew.ContainsKey(crew.Tmdb_Id))
                            {
                                secondCastAndCrew[crew.Tmdb_Id][crew.Name].Add("director"); // Voeg toe aan bestaande persoon.
                            }
                            else
                            {
                                Dictionary<string, List<string>> personDictionary = new Dictionary<string, List<string>>
                                {
                                    { crew.Name, new List<string> { "director" } }
                                };

                                secondCastAndCrew[crew.Tmdb_Id] = personDictionary; // Voegt nieuwe regisseur toe.
                            }

                        }

                    }

                    return secondCastAndCrew;
                }

            }
                        
            return null; // Als er een fout is opgetreden of geen credits worden gevonden, wordt er null gereturned.

        }

        public async Task<Movie?> GetMovieByTmdbId(int tmdbId)
        {
            // get the movie details from this url
            // https://api.themoviedb.org/3/movie/{movie_id}
            // details on how to use:
            // https://developer.themoviedb.org/reference/movie-details
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://api.themoviedb.org/3/movie/{tmdbId}"); // API-verzoek opbouwen.
            request.Headers.Add("Authorization", headerApiKey); // Auth-header toevoegen.            
            HttpResponseMessage response = _httpClient.Send(request); // Verzend het verzoek.
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync(); // Lees de content.
                Movie movieresult = JsonSerializer.Deserialize<Movie>(responseContent); // Deserialize naar Movie.

                if (!string.IsNullOrEmpty(movieresult.Year) && movieresult.Year.Length >= 4)
                {
                    movieresult.Year = movieresult.Year.Substring(0, 4); // enkel het jaar zal overhouden worden.
                }

                if (movieresult.Score.HasValue) // Hier wordt er nagegaan of de score uit de API wel degelijk een waarde heeft.
                {
                    movieresult.Score = Math.Round(movieresult.Score.Value, 2); // Rond score af op 2 decimalen.
                }

                return movieresult;

            }
            else
            {
                return null; // Geen succesvol antwoord, return null.
            }

        }

    }

}
