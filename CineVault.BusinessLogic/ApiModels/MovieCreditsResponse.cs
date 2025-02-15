using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.ApiModels
{
    public class MovieCreditsResponse
    {
        [JsonPropertyName("cast")]
        public List<Cast> Cast { get; set; }

        [JsonPropertyName("crew")]
        public List<Crew> Crew { get; set; }
    }
}
