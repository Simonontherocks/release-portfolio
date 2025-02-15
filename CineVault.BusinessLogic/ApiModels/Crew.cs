using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.ApiModels
{
    public class Crew
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("job")]
        public string Job { get; set; }
    }
}
