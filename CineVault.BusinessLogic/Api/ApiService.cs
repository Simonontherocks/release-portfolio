using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.Service
{
    public class ApiService
    {
        public void SplitActorsIntoList(string actorsBundled)
        {
            string[] temporaryArray = actorsBundled.Split(',');
            // TODO: errors wegwerken

            //foreach (string actor in temporaryArray)
            //{
            //    if (!listOfApiActors.Contains(actor.Trim().ToLower()))
            //    {
            //        listOfApiActors.Add(actor.Trim().ToLower());
            //    }

            //}

        }

        /*
         * In deze methode wordt gebruik gemaakt van de API en de verkregen-key API van OMDB.
         * Hier zal een request gedaan worden naar de API van OMDB.
         */

        public void MovieTest(string movieTitle) // Testen op resultaat voor film
        {
            string apiKey = "2961a242"; // ApiKey
            string apiUrl = $"http://www.omdbapi.com/?t={Uri.EscapeDataString(movieTitle)}&apikey={apiKey}";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);

            Task<HttpResponseMessage> responseTask = client.GetAsync(apiUrl);
            responseTask.Wait();

            HttpResponseMessage responseAnswer = responseTask.Result;
            Console.WriteLine("Statuscode" + responseAnswer.StatusCode);

            // TODO: errors wegwerken

            //if (responseAnswer.IsSuccessStatusCode)
            //{
            //    Task<MovieInformationApi> openenVanBriefTaak = responseAnswer.Content.ReadFromJsonAsync<MovieInformationApi>();
            //    openenVanBriefTaak.Wait();
            //    MovieInformationApi inhoud = openenVanBriefTaak.Result;

            //    Console.WriteLine(inhoud.Actors);
            //    //Console.WriteLine(inhoud.Title);
            //    //Console.WriteLine(inhoud.Year);
            //    //Console.WriteLine(inhoud.Director);

            //    SplitActorsIntoList(inhoud.Actors);
            //}
        }
    }
}
