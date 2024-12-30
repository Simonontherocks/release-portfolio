using System.Data;

namespace CineVault.BusinessLogic.Models
{
    public class IMDBEntry
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public IMDBType Type { get; set; }
    }

}
