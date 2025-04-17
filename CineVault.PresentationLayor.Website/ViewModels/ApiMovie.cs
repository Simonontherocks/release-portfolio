using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CineVault.PresentationLayer.Website.ViewModels
{
    public class ApiMovie
    {
        #region Properties

        public int IMDBId { get; set; }

        public string Title { get; set; }

        public string? Year { get; set; }

        #endregion
    }
}
